using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCharacterController : MonoBehaviour
{
    protected Animator animator;

    protected CharacterStatistics myStats;

    protected string lastAnimationName;
    protected string lastWeaponUsedName;
    protected string lastTargetName;
    protected float lastAttackTime;


    // Update is called once per frame
    public void Update()
    {
        ManageMovement();
        ManageJumpAndGravity();
        ManageAttack();
    }

    public virtual void ManageMovement() { }
    public virtual void ManageJumpAndGravity() { }
    public virtual void ManageAttack() { }

    public virtual void TargetTouched(string weaponName, GameObject target) {
        float currentTime = Time.time;
        string currentAnimationName = (animator.GetCurrentAnimatorClipInfo(0))[0].clip.name;
        string currentTargetName = target.name;

        if (animator.GetBool("isAttacking") && (
                currentTime - lastAttackTime > 1.0f
                || !lastTargetName.Equals(currentTargetName) 
                || !currentAnimationName.Equals(lastAnimationName)
                || (!weaponName.Equals(lastWeaponUsedName) && currentAnimationName.Contains("DoubleAttack"))
            )
        )
        {
            lastAnimationName = currentAnimationName;
            lastWeaponUsedName = weaponName;
            lastTargetName = currentTargetName;
            lastAttackTime = currentTime;

            target.GetComponent<CharacterStatistics>().TakeDamage(myStats.damage);
        }
    }

    public virtual void ManageDefence() { }
}
