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

    public virtual void TakeDamage(int damage)
    {
        // scala il danno preso in base all'armatura del personaggio
        damage -= myStats.armour;
        // se il danno è non positivo, lo setto a un minimo di 1
        if (damage <= 0)
            damage = 1;

        // Scalo gli hp in baso al danno preso
        myStats.hp -= damage;

        // Se gli hp sono non positivi, lancio la gestione della morte
        if (myStats.hp <= 0)
            Die();
    }

    public virtual void Die()
    {
        // La morte cambia a seconda del character
        // bisognerà fare l'override del metodo
    }

    public virtual void ManageMovement()
    {
    }

    public virtual void ManageJumpAndGravity()
    {
    }

    public virtual void ManageAttack()
    {
    }

    public virtual void TargetTouched(string weaponName, GameObject target)
    {
        float currentTime = Time.time;
        string currentAnimationName = (animator.GetCurrentAnimatorClipInfo(0))[0].clip.name;
        string currentTargetName = target.name;


        if (animator.GetBool("isAttacking") &&
            currentAnimationName.Contains("Attack") && (
                currentTime - lastAttackTime > 1.0f
                || !lastTargetName.Equals(currentTargetName)
                || !currentAnimationName.Equals(lastAnimationName)
                || (!weaponName.Equals(lastWeaponUsedName) && currentAnimationName.Contains("DoubleAttack"))
            )
        )
        {
            /*    print($"target Old VS New: {lastTargetName} | {currentTargetName} = {!currentTargetName.Equals(lastTargetName)}");
                print($"weapon Old VS New: {lastWeaponUsedName} | {weaponName} = {!weaponName.Equals(lastWeaponUsedName)}");
                print($"time:{(currentTime - lastAttackTime > 1.0f)}");
                print($"animation Old VS New: {lastAnimationName} | {currentAnimationName} = {!currentAnimationName.Equals(lastAnimationName)}");
            */
            lastAnimationName = currentAnimationName;
            lastWeaponUsedName = weaponName;
            lastTargetName = currentTargetName;
            lastAttackTime = currentTime;

            target.GetComponent<GameCharacterController>().TakeDamage(myStats.damage);
        }
    }

    public virtual void ManageDefence()
    {
    }
}