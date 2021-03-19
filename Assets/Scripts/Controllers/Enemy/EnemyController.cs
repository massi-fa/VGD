using UnityEngine;
using UnityEngine.AI;

namespace Controllers.Enemy
{
    public class EnemyController : GameCharacterController
    {

        public float lookRadius = 10f;

        private NavMeshAgent navigationMeshAgent;
        private NavMeshPath navMeshPath;
        public Transform playerTransform;

        public Vector3 originalPosition;

        private float deadAnimationTime;
    

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            flashWhenHit = true;
        
            navigationMeshAgent = GetComponent<NavMeshAgent>();
            navMeshPath = new NavMeshPath();
        
            playerTransform = PlayerTracker.instance.player.transform;
            originalPosition = transform.position;

            //
            RaycastHit hit;
            if (Physics.Raycast (transform.position, Vector3.down, out hit, 100, LayerMask.GetMask("Ground"))) {
                var distanceToGround = hit.distance;
                originalPosition.y -= distanceToGround;
            }
            //
            //print("transform.position " + transform.position );
            // print("originalPosition " + originalPosition );


            var clips = animator.runtimeAnimatorController.animationClips;
            foreach (var clip in clips)
            {
                if (clip.name.Contains("Dead"))
                    deadAnimationTime = clip.length;
            }
        }

        protected override void ManageMovement()
        {
            base.ManageMovement();

            // distanza fra il nemico e il player
            float distanceFromPlayer = Vector3.Distance(transform.position, playerTransform.position);

            // se il nemico vede il player
            if (distanceFromPlayer <= lookRadius
                && navigationMeshAgent.CalculatePath(playerTransform.position, navMeshPath)
                && navMeshPath.status == NavMeshPathStatus.PathComplete)
            {
        
                // il nemico viene aggrato
                navigationMeshAgent.SetPath(navMeshPath);
                animator.SetBool(IsMoving, true);
        
            }
            // altrimenti se non lo vede
            else
            {
                // print("transform.position" + transform.position);
                // print("position " + originalPosition);
                // Se è lontano al punto originale
                if (Vector3.Distance(transform.position, originalPosition) > 2.0f)
                {
                    // Torna al punto originale
                    animator.SetBool(IsMoving, true);
                    navigationMeshAgent.SetDestination(originalPosition);
                }
                // Altrimenti sta fermo
                else
                {
                    animator.SetBool(IsMoving, false);
                }
            }
        }


        protected override void ManageAttack()
        {
            base.ManageAttack();

            // distanza fra il nemico e il player
            float distanceFromPlayer = Vector3.Distance(transform.position, playerTransform.position);

            // se il nemico è abbastanza vicino
            if (distanceFromPlayer <= navigationMeshAgent.stoppingDistance)
            {
                // smette di muoversi
                animator.SetBool(IsMoving, false);

                // Attacca il player
                animator.SetBool(IsAttacking, true);

                // Ruota verso il player
                FacePlayer();
            }
            else
            {
                animator.SetBool(IsAttacking, false);
            }
        }

        private void FacePlayer()
        {
            // ricavo la direzione verso il player
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            // trova la rotazione che deve fare per guardare il player
            // Quaternion lookRotation = Quaternion.LookRotation(direction)
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            // applica quella rotazione in maniera più smooth
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        protected override void Die()
        {
            base.Die();

            // Animazione
            animator.SetBool(IsDead, true);

            // per sicurezza resetto il colore prima di distruggere l'oggetto
            //changeColorMaterial.ResetColor();

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
}
