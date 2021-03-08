using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ProtagonistController : GameCharacterController
{
    CharacterController controller;
    Transform groundCheckTransform;

    public float velocity = 6f;
    public float gravity = -9.81f;
    public float mouseSensitivity = 20.0f;
    public float turnSmoothTime = 0.2f;
    public float turnSmoothVelocity;
    public Camera eyes;
    public float groundDistance = 0.4f;
    public LayerMask layerToCheck;
    public float jumpHeight = 3f;

    public Vector3 verticalVelocityVector;
    bool isGrounded;

    bool secondAttackClickLock;
    public bool amIAttacking;


    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
        myStats = GetComponent<CharacterStatistics>();

        controller = GetComponent<CharacterController>();
        groundCheckTransform = transform.Find("GroundCheck");



        // Aggiusta l'altezza iniziale del character controller
        float correctHeight = controller.center.y + controller.skinWidth;
        controller.center = new Vector3(0, correctHeight, 0);
    }


    public override void ManageMovement()
    {
        base.ManageMovement();

        Cursor.lockState = CursorLockMode.Locked;

        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");

        // ricavo la direzione in cui voglio andare
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // se esiste questa direzione (il pg non è fermo)
        if (direction.magnitude > 0f)
        {
            animator.SetBool("isWalking", true);

            // calcolo l'angolo tra il forward e la direzione in cui voglio andare e lo converto da radianti a gradi
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            // Considero anche l'angolo della telecamera
            targetAngle += eyes.transform.eulerAngles.y;
            // ci si muove graduatamente dall'angolo attuale all'angolo finale
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            // cammbio la rotazione del personaggio verso il punto in cui vuole andare
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z)), Time.deltaTime * 5f);
            // applico il movimento
            Vector3 moveDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
            controller.Move(moveDirection.normalized * velocity * Time.deltaTime);
        }
        else
            animator.SetBool("isWalking", false);
    }

    public override void ManageJumpAndGravity()
    {
        base.ManageJumpAndGravity();

        // Calcola se il pg è a terra
        isGrounded = Physics.CheckSphere(groundCheckTransform.position, groundDistance, layerToCheck);
        // Controllo input utente per saltare
        bool wantToJump = Input.GetButtonDown("Jump");

        // Se il pg è a terra e il vettore velocità non è stato resettato
        if (isGrounded && verticalVelocityVector.y < 0f)
        {
            // resetto il vettore velocità
            verticalVelocityVector.y = -0.05f;
        }

        // Se l'utente vuole saltare e se il pg è a terra
        if (wantToJump && isGrounded)
        {
            // aggiorno il vetotre velocità
            verticalVelocityVector.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Applico la gravità
        verticalVelocityVector.y += 2 * gravity * Time.deltaTime;

        // Aggiorno la posizione del pg
        controller.Move(verticalVelocityVector * Time.deltaTime);
    }

    public override void ManageAttack()
    {
        base.ManageAttack();

        bool wantToAttack = Input.GetButtonDown("Fire1");

        if (wantToAttack)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack00"))
                animator.SetBool("clickedForTheNextAttack", true);
            else
                animator.SetBool("isAttacking", true);
        }
    }

    public override void ManageDefence()
    {
        base.ManageDefence();

        bool wantToDefend = Input.GetButtonDown("Fire2");
        animator.SetBool("isDefending", wantToDefend);
    }

    public override void TargetTouched(string weaponName, GameObject target)
    {
        base.TargetTouched(weaponName, target);
    }

}
