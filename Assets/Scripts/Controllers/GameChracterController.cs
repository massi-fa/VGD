using UnityEngine;
using System.Collections;

public class GameCharacterController : MonoBehaviour
{
    protected Animator animator;
    private Animation _singleAnimationInsteadOfAnimator;
    private CharacterStatistics _myStats;

    private int _lastStateAnimationName;
    private string _lastWeaponUsedName;
    private string _lastTargetName;
    private   float _lastAttackTime;
    private string _lastClipAnimationName;
    public float countdownAttack = 1.0f;

    private  ChangeColorMaterialTemporary _changeColorMaterial;
    
    protected  static readonly int IsAttacking = Animator.StringToHash("isAttacking");
    protected  static readonly int IsMoving = Animator.StringToHash("isMoving");
    protected  static readonly int IsDead = Animator.StringToHash("isDead");

    public bool flashWhenHit;
    public bool isAnimated = true;

    private GameCharacterHealthBarController _healtBarControllerScript;
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        _singleAnimationInsteadOfAnimator = GetComponent<Animation>();
        _myStats = GetComponent<CharacterStatistics>();
        _changeColorMaterial = GetComponent<ChangeColorMaterialTemporary>();
        _healtBarControllerScript = GetComponent<GameCharacterHealthBarController>();
    }

    protected virtual void Update()
    {
        if (isAnimated)
        {
            if (animator.GetBool(IsDead)) return;
            ManageJumpAndGravity();
            ManageMovement();
            ManageAttack();
        }
        ManageAttack();
    }
    
    public void TargetTouched(string myObjectPieceCollidedName, GameObject otherObject)
    {
        var animatedTarget = otherObject.GetComponent<GameCharacterController>();
        if (animatedTarget == null || !animatedTarget.isAnimated)
            return;
        
        bool isAttacking;
        int currentStateAnimationName;
        string currentClipAnimationName;
        var currentTime = Time.time;
        
        if (isAnimated)
        {
            currentClipAnimationName = (animator.GetCurrentAnimatorClipInfo(0))[0].clip.name;
            currentStateAnimationName = animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
            isAttacking = animator.GetBool(IsAttacking);
        }
        else
        {
            AnimationClip clip;
            currentStateAnimationName = (clip = _singleAnimationInsteadOfAnimator.clip).GetHashCode();
            currentClipAnimationName = clip.name;
            isAttacking = _singleAnimationInsteadOfAnimator.isPlaying;
        }
        var currentTargetName = otherObject.name;
    
        //print($"target Old VS New: {_lastTargetName} | {currentTargetName} = {!currentTargetName.Equals(_lastTargetName)}");
        //print($"weapon Old VS New: {_lastWeaponUsedName} | {myObjectPieceCollidedName} = {!myObjectPieceCollidedName.Equals(_lastWeaponUsedName)}");
        //print($"time: current - old : {(currentTime - _lastAttackTime > 1.0f)}");
        //print($"animation Old VS New: {_lastAnimationName} | {currentStateAnimationName} = {currentTargetName != _lastTargetName}");
        //print("isAttacking? " + isAttacking);
        //print(currentStateAnimationName + " containsAttack? " + currentStateAnimationName.Contains("Attack"));
        
        
        if (isAttacking &&
            currentClipAnimationName.Contains("Attack") && (
                currentTime - _lastAttackTime > countdownAttack
                || !currentTargetName.Equals(_lastTargetName)
                || currentStateAnimationName != _lastStateAnimationName
                || (!myObjectPieceCollidedName.Equals(_lastWeaponUsedName) && currentClipAnimationName.Contains("DoubleAttack"))
            )
        )
        {
            //print($"target Old VS New: {_lastTargetName} | {currentTargetName} = {!currentTargetName.Equals(_lastTargetName)}");
            //print($"weapon Old VS New: {_lastWeaponUsedName} | {myObjectPieceCollidedName} = {!myObjectPieceCollidedName.Equals(_lastWeaponUsedName)}");
            //print($"time: {currentTime} - {_lastAttackTime} > {countdownAttack} : {(currentTime - _lastAttackTime > 1.0f)}");
            //print($"animation Old VS New: {currentStateAnimationName} | {currentStateAnimationName} = {currentStateAnimationName != _lastStateAnimationName}");
            //print("isAttacking? " + isAttacking);
            //print(currentStateAnimationName + " containsAttack? " + currentStateAnimationName.Contains("Attack"));
        /*    print(currentTime - _lastAttackTime > countdownAttack);
            print(!currentTargetName.Equals(_lastTargetName));
            print($"{currentStateAnimationName} | {_lastStateAnimationName} := {(currentStateAnimationName != _lastStateAnimationName)}");
            print(!myObjectPieceCollidedName.Equals(_lastWeaponUsedName) && currentClipAnimationName.Contains("DoubleAttack"));
            print("_________________________-");*/
            _lastClipAnimationName = currentClipAnimationName;
            _lastStateAnimationName = currentStateAnimationName;
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
    
    
    #region FixedStatus
    
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

        // update the health bar
        if (_healtBarControllerScript != null)
            _healtBarControllerScript.TakeDamage(damage);
        
        // Se gli hp sono non positivi, lancio la gestione della morte
        if (_myStats.hp <= 0)
            Die();
    }
    
    public void TakeHitPoints(int bonusHp) {
        // Se sono full esco
        if (_myStats.hp == _myStats.maxHp)
            return;

        // Se col bonus dovessi sforare il limite massimo
        if (_myStats.hp + bonusHp > _myStats.maxHp)
            // imposto il bonus come la differenza fra la vita massima e la vita attuale
            bonusHp = _myStats.maxHp - _myStats.hp;
        
        // aggiorno la vita attuale col bonus
        _myStats.hp += bonusHp;
        
        // Aggiorno la barra della vita
        if (_healtBarControllerScript != null)
            _healtBarControllerScript.RestoreHealt(bonusHp);
    }


    public void UpgradeArmour(int bonusArmour) {
        _myStats.armour += bonusArmour;
    }

    public void UpgradeDamage(int bonusDamange)
    {
        _myStats.damage += bonusDamange;
    }
    #endregion

    #region TemporaneousStatus
    public IEnumerator TemporaneousDamageBuff(int bonusDamage, int seconds) {
        _myStats.damage += bonusDamage;
        yield return Waiter.Active(seconds);
        _myStats.damage -= bonusDamage;
    }

    public IEnumerator TemporaneousArmourBuff(int bonusArmour, int seconds) {
        _myStats.armour += bonusArmour;
        yield return Waiter.Active(seconds);
        _myStats.armour -= bonusArmour;
    }
    #endregion
}