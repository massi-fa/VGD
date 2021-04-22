using System.Collections;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class ProtagonistController : GameCharacterController
{
    private CharacterController _controller;
    private Transform _groundCheckTransform;

    [Header("Camera")] [Tooltip("Assegnagli la Main Camera")]
    public Camera eyes;

    [Header("Controllo se è a terra")]
    [Tooltip("Raggio della sfera che verifica se vi è terreno ai piedi del player (LASCIA 0.2f)")]
    [Min(0.1f)]
    public float groundDistance = 0.2f;

    [Tooltip("Layer usato per capire se un oggetto è di tipo terreno\n(LASCIA \"Ground\")")]
    public LayerMask layerToCheck;

    [Header("Movimento")] [Tooltip("Velocità del player")] [Min(1f)]
    public float velocity = 6f;

    [Tooltip("Tempo di rotazione del player\n(LASCIA 0.19f)")] [Min(0.1f)]
    public float turnSmoothTime = 0.2f;

    private float _turnSmoothVelocity;
    
    [Header("Dash/Scatto")]
    [Tooltip("Potenza dello scatto")] 
    public float dashForce = 5f;

    /*[Tooltip("Frequenza aggiornamento dash\n(25 è un buon valore)")]
    public float dashSmoothTime = 25f;*/

    [Tooltip("Ogni quanti secondi si può effettuare il dash (scatto)")]
    public float dashCountDown = 3f;

    private float _lastTimeDash = 0f;


    [Header("Salto")] [Tooltip("Valore della gravità applicata sul player,\nDEVE essere negativo (e.g. -9.81)")]
    public float gravity = -9.81f;

    [Tooltip("Forza nel salto")] [Min(1f)] public float jumpHeight = 3f;
    private bool _forcedJump;
    private float _jumperPower;

    private Vector3 _verticalVelocityVector;

    [Tooltip("Velocità verticale negativa da raggiungere per morire in aria\n(-40f è un buon valore)")]
    public float verticalVelocityLimitBeforeDying = -40f;

    private bool _isGrounded;

    private static readonly int IsDefending = Animator.StringToHash("isDefending");
    private static readonly int ClickedForTheNextAttack = Animator.StringToHash("clickedForTheNextAttack");

    private GameCharacterDashBar _dashBar;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        _controller = GetComponent<CharacterController>();
        _groundCheckTransform = transform.Find("GroundCheck");
        _dashBar = GetComponent<GameCharacterDashBar>();
        _dashBar.dashCountDown = dashCountDown;

        // Aggiusta l'altezza iniziale del character controller
        float correctHeight = _controller.center.y + _controller.skinWidth;
        _controller.center = new Vector3(0, correctHeight, 0);
    }

    protected override void ManageMovement()
    {
        base.ManageMovement();

        Cursor.lockState = CursorLockMode.Locked;

        var vertical = Input.GetAxisRaw("Vertical");
        var horizontal = Input.GetAxisRaw("Horizontal");

        // ricavo la direzione in cui voglio andare
        var direction = new Vector3(horizontal, 0f, vertical).normalized;

        // se esiste questa direzione (il player ha intenzione di muoversi)
        if (direction.magnitude > 0f)
        {
            animator.SetBool(IsMoving, true);

            // calcolo l'angolo tra il forward e la direzione in cui voglio andare e lo converto da radianti a gradi
            var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            // Considero anche l'angolo della telecamera
            targetAngle += eyes.transform.eulerAngles.y;
            // ci si muove graduatamente dall'angolo attuale all'angolo finale
            var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity,
                turnSmoothTime);

            // cammbio la rotazione del personaggio verso il punto in cui vuole andare
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // applico il movimento
            var moveDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;

            _controller.Move(moveDirection.normalized * (velocity * Time.deltaTime));
        }
        else
            animator.SetBool(IsMoving, false);

        // scatto
        /*if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            var x = transform.forward * 5f;
            var y = Vector3.SmoothDamp(transform.position, x, ref test, 25f*Time.deltaTime);
            _controller.Move(x);
        } */
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            var dashResult = transform.forward * dashForce;
            var positionUpped = transform.position + transform.up * _controller.height / 2;

            var currentTime = Time.time;
            var canDash = currentTime - _lastTimeDash >= dashCountDown || _lastTimeDash == 0f;
            // Se non è passato abbastanza tempo esce
            if (!canDash) return;

            // Se c'è un qualche ostacolo durante il tragitto da percorrere
            if (Physics.Linecast(positionUpped, positionUpped + dashResult, out var info))
            {
                // Si controlla se non si è troppo vicini ad un oggetto
                if (Vector3.Distance(positionUpped, info.point) < 1f) return;
                /*var distanceCollisionFromMax = Vector3.Distance(info.point, info.collider.bounds.max);
                x -= transform.forward * (distanceCollisionFromMax + 0.1f);*/
            }

            _dashBar.ResetBar();
            _lastTimeDash = currentTime;
            _controller.Move(dashResult);
        }
    }

    protected override void ManageJumpAndGravity()
    {
        base.ManageJumpAndGravity();

        // Calcola se il pg è a terra
        _isGrounded = Physics.CheckSphere(_groundCheckTransform.position, groundDistance, layerToCheck);
        // Controllo input utente per saltare
        var wantToJump = Input.GetButtonDown("Jump");

        // Se il pg è a terra e il vettore velocità non è stato resettato
        if (_isGrounded && _verticalVelocityVector.y < 0f)
        {
            // resetto il vettore velocità
            _verticalVelocityVector.y = -0.05f;
        }

        // Se l'utente vuole saltare e se il pg è a terra
        if ((wantToJump || _forcedJump) && _isGrounded)
        {
            if (_forcedJump)
            {
                _forcedJump = false;
                // aggiorno il vettore velocità
                _verticalVelocityVector.y = Mathf.Sqrt((jumpHeight + _jumperPower) * -2f * gravity);
                _jumperPower = 0f;
            }
            else
            {
                // aggiorno il vettore velocità
                _verticalVelocityVector.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        // Applico la gravità
        _verticalVelocityVector.y += 2 * gravity * Time.deltaTime;

        // Aggiorno la posizione del pg
        _controller.Move(_verticalVelocityVector * Time.deltaTime);

        // Controllo che non stia cadendo nel vuoto
        // e se ha oggetti sotto
        if (_verticalVelocityVector.y <= verticalVelocityLimitBeforeDying &&
            !Physics.Raycast(transform.position, Vector3.down, 5))
            //if(verticalVelocityVector.y <= verticalVelocityLimitBeforeDying)
        {
            _healtBarControllerScript.TakeDamage(_myStats.hp);
            Die();
        }
    }

    public void CollisionWithJumper(float jumperPower)
    {
        _jumperPower = jumperPower;
        _forcedJump = true;
    }

    protected override void ManageAttack()
    {
        base.ManageAttack();

        var wantToAttack = Input.GetButtonDown("Fire1");

        if (!wantToAttack) return;

        animator.SetBool(
            animator.GetCurrentAnimatorStateInfo(0).IsName("Attack00") ? ClickedForTheNextAttack : IsAttacking,
            true);
    }

    protected override void ManageDefence()
    {
        base.ManageDefence();

        bool wantToDefend = Input.GetButtonDown("Fire2");
        animator.SetBool(IsDefending, wantToDefend);
    }

    protected override void Die()
    {
        base.Die();
        StartCoroutine(SceneTransitioner.GetInstance().GoToSceneAfterNSeconds(indexScene: 2, deadAnimationTime));
    }

    public override IEnumerator TemporaneousSpeedBuff(int bonusSpeed, int seconds)
    {
        //yield return base.TemporaneousSpeedBuff(bonusSpeed, seconds);

        velocity += bonusSpeed;
        jumpHeight += (float) bonusSpeed / 2;
        yield return Waiter.Active(seconds);
        velocity -= bonusSpeed;
        jumpHeight -= (float) bonusSpeed / 2;
    }
}