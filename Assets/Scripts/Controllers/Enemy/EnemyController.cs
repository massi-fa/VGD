using UnityEngine;
using UnityEngine.AI;

namespace Controllers.Enemy
{
    public class EnemyController : GameCharacterController
    {

        public float lookRadius = 10f;

        private NavMeshAgent _navigationMeshAgent;
        private NavMeshPath _navMeshPath;
        public Transform playerTransform;

        public Vector3 originalPosition;

        private float _deadAnimationTime;
    

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // I nemici flashano se colpiti
            flashWhenHit = true;
        
            // Inizializza nav agent variable e crea un nuovo nev mesh path
            _navigationMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshPath = new NavMeshPath();
        
            // Traccio il player
            playerTransform = PlayerTracker.instance.player.transform;
            
            // Controllo la distanza da terra per approssimare la posizione originale
            originalPosition = transform.position;
            if (Physics.Raycast (transform.position, Vector3.down, out var hit, 100, LayerMask.GetMask("Ground"))) {
                var distanceToGround = hit.distance;
                originalPosition.y -= distanceToGround;
            }
            
            // Calcola ora quanto dura la lunghezza della clip di morte
            var clips = animator.runtimeAnimatorController.animationClips;
            foreach (var clip in clips)
            {
                if (clip.name.Contains("Dead"))
                    _deadAnimationTime = clip.length;
            }
        }

        protected override void ManageMovement()
        {
            base.ManageMovement();

            // distanza fra il nemico e il player
            var distanceFromPlayer = Vector3.Distance(transform.position, playerTransform.position);

            // se il nemico vede il player
            if (distanceFromPlayer <= lookRadius
                && _navigationMeshAgent.CalculatePath(playerTransform.position, _navMeshPath)
                && _navMeshPath.status == NavMeshPathStatus.PathComplete)
            {
        
                // il nemico viene aggrato
                _navigationMeshAgent.SetPath(_navMeshPath);
                animator.SetBool(IsMoving, true);
        
            }
            // altrimenti se non lo vede
            else
            {
                // Se è lontano al punto originale
                if (Vector3.Distance(transform.position, originalPosition) > 2.0f)
                {
                    // Torna al punto originale
                    animator.SetBool(IsMoving, true);
                    _navigationMeshAgent.SetDestination(originalPosition);
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
            var distanceFromPlayer = Vector3.Distance(transform.position, playerTransform.position);

            // se il nemico è abbastanza vicino
            if (distanceFromPlayer <= _navigationMeshAgent.stoppingDistance)
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
            var direction = (playerTransform.position - transform.position).normalized;
            // trova la rotazione che deve fare per guardare il player
            var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
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
            Destroy(gameObject,_deadAnimationTime);
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
