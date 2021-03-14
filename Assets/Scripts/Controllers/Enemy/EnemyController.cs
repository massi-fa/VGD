using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : GameCharacterController
{

    public float lookRadius = 10f;

    private NavMeshAgent navigationMeshAgent;
    private Transform playerTransform;

    private Vector3 originalPosition;
    private  ChangeColorMaterialTemporary changeColorMaterial;
    private float deadAnimationTime;
   
    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
        myStats = GetComponent<CharacterStatistics>();

        navigationMeshAgent = GetComponent<NavMeshAgent>();
        playerTransform = PlayerTracker.instance.player.transform;
        originalPosition = transform.position;

        changeColorMaterial = GetComponent<ChangeColorMaterialTemporary>();

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name.Contains("Dead"))
                deadAnimationTime = clip.length;
        }
    }

    public override void ManageMovement()
    {
        base.ManageMovement();

        // distanza fra il nemico e il player
        float distanceFromPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // se il nemico vede il player
        if (distanceFromPlayer <= lookRadius)
        {
            // il nemico viene aggrato
            navigationMeshAgent.SetDestination(playerTransform.position);
            animator.SetBool("isMoving", true);
        }
        // altrimenti se non lo vede
        else
        {
            // Se è lontano al punto originale
            if (Vector3.Distance(transform.position, originalPosition) > 2.0f)
            {
                // Torna al punto originale
                animator.SetBool("isMoving", true);
                navigationMeshAgent.SetDestination(originalPosition);
            }
            // Altrimenti sta fermo
            else
            {
                animator.SetBool("isMoving", false);
            }
        }
    }


    public override void ManageAttack()
    {
        base.ManageAttack();

        // distanza fra il nemico e il player
        float distanceFromPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // se il nemico è abbastanza vicino
        if (distanceFromPlayer <= navigationMeshAgent.stoppingDistance)
        {
            // smette di muoversi
            animator.SetBool("isMoving", false);

            // Attacca il player
            animator.SetBool("isAttacking", true);

            // Ruota verso il player
            FaceTarget();
        }
        else
        {
            animator.SetBool("isAttacking", false);
        }
    }

    /* public void Attack()
     {

     }*/

    public void FaceTarget()
    {
        // ricavo la direzione verso il player
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        // trova la rotazione che deve fare per guardare il player
        // Quaternion lookRotation = Quaternion.LookRotation(direction)
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        // applica quella rotazione in maniera più smooth
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    public override void TargetTouched(string weaponName, GameObject target)
    {
        base.TargetTouched(weaponName, target);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        changeColorMaterial.FlashColor();
    }
    public override void Die()
    {
        base.Die();

        // Animazione
        animator.SetBool("isDead", true);

        // per sicurezza resetto il colore prima di distruggere l'oggetto
        changeColorMaterial.ResetColor();

        // Distruggi il gameobject
        Destroy(gameObject,deadAnimationTime);
    }

    #region EnemyVisionOnEditor
    private void OnDrawGizmosSelected()
    {
        // Disegna nell'editor una sfera rossa che rappresenterà il volume il cui il nemico può vedere
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
    #endregion

}
