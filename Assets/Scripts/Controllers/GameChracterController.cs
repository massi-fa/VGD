using UnityEngine;

public class GameCharacterController : MonoBehaviour
{
    protected Animator animator;
    private Animation _singleAnimationInsteadOfAnimator;
    private CharacterStatistics _myStats;

    private string _lastAnimationName;
    private string _lastWeaponUsedName;
    private string _lastTargetName;
    private   float _lastAttackTime;
    public float countdownAttack = 1.0f;

    private  ChangeColorMaterialTemporary _changeColorMaterial;
    
    protected  static readonly int IsAttacking = Animator.StringToHash("isAttacking");
    protected  static readonly int IsMoving = Animator.StringToHash("isMoving");
    protected  static readonly int IsDead = Animator.StringToHash("isDead");

    public bool flashWhenHit;
    public bool isAnimated = true;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        _singleAnimationInsteadOfAnimator = GetComponent<Animation>();
        _myStats = GetComponent<CharacterStatistics>();
        _changeColorMaterial = GetComponent<ChangeColorMaterialTemporary>();
    }

    protected virtual void Update()
    {
        if (isAnimated)
        {
            if (animator.GetBool(IsDead)) return;
            ManageMovement();
            ManageJumpAndGravity();
            ManageAttack();
        }
        ManageAttack();
    }

    private void TakeDamage(int damage)
    {
        // Se il danno è non positivo esco
        if (damage <= 0)
            return;

        // scala il danno preso in base all'armatura del personaggio
        damage -= _myStats.armour;
        // se il danno è non positivo, lo setto a un minimo di 1
        if (damage <= 0)
            damage = 1;

        // cambio il colore
        if(flashWhenHit)
            _changeColorMaterial.FlashColor();
            
        // Scalo gli hp in baso al danno preso
        _myStats.hp -= damage;

        // Se gli hp sono non positivi, lancio la gestione della morte
        if (_myStats.hp <= 0)
            Die();
    }
    public void TargetTouched(string myObjectPieceCollidedName, GameObject otherObject)
    {
        var animatedTarget = otherObject.GetComponent<GameCharacterController>();
        if (animatedTarget == null || !animatedTarget.isAnimated)
            return;
        
        bool isAttacking;
        string currentAnimationName;
        
        float currentTime = Time.time;
        
        if (isAnimated)
        {
            currentAnimationName = (animator.GetCurrentAnimatorClipInfo(0))[0].clip.name;
            isAttacking = animator.GetBool(IsAttacking);
        }
        else
        {
            currentAnimationName = _singleAnimationInsteadOfAnimator.clip.name;
            isAttacking = _singleAnimationInsteadOfAnimator.isPlaying;
        }
        string currentTargetName = otherObject.name;
    /*
        print($"target Old VS New: {lastTargetName} | {currentTargetName} = {!currentTargetName.Equals(lastTargetName)}");
        print($"weapon Old VS New: {lastWeaponUsedName} | {myObjectPieceCollidedName} = {!myObjectPieceCollidedName.Equals(lastWeaponUsedName)}");
        print($"time: current - old : {(currentTime - lastAttackTime > 1.0f)}");
        print($"animation Old VS New: {lastAnimationName} | {currentAnimationName} = {!currentAnimationName.Equals(lastAnimationName)}");
        print("isAttacking? " + isAttacking);
        print(currentAnimationName + " containsAttack? " + currentAnimationName.Contains("Attack"));
        
      */  
        if (isAttacking &&
            currentAnimationName.Contains("Attack") && (
                currentTime - _lastAttackTime > countdownAttack
                || !_lastTargetName.Equals(currentTargetName)
                || !currentAnimationName.Equals(_lastAnimationName)
                || (!myObjectPieceCollidedName.Equals(_lastWeaponUsedName) && currentAnimationName.Contains("DoubleAttack"))
            )
        )
        {
            
            _lastAnimationName = currentAnimationName;
            _lastWeaponUsedName = myObjectPieceCollidedName;
            _lastTargetName = currentTargetName;
            _lastAttackTime = currentTime;
            
            animatedTarget.TakeDamage(_myStats.damage);

        }
    }
    
    protected virtual void ManageAttack() { }

    protected virtual void ManageDefence() { }

    protected virtual void ManageMovement() { }

    protected virtual void ManageJumpAndGravity() { }
    
    protected virtual  void Die() {}
}