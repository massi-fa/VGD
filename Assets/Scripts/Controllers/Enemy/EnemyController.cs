using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Controllers.Enemy
{
    public class EnemyController : GameCharacterController
    {
        [Tooltip("Distanza necessaria per aggrare del nemico")]
        public float lookRadius = 10f;
        [Tooltip("Distanza dopo la quale usa gli attacchi melee")]
        public float maxDistanceForMeleeAttack = 2f;
        private NavMeshAgent _navigationMeshAgent;
        private NavMeshPath _navMeshPath;
        private Transform _playerTransform;

        private Vector3 _originalPosition;

        //private float _deadAnimationTime;
        [Tooltip("Spuntarla se il nemico ha attacchi range (I PREFAB SON GIA' PRONTI, NON TOCCARE)")]
        public bool hasRangedAttacks;
        [Tooltip("Distanza dopo la quale usa gli attacchi a distanza")]
        public float maxDistanceForRangedAttack;
        private static readonly int Melee = Animator.StringToHash("Melee");
        private static readonly int Distance = Animator.StringToHash("Distance");

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
            _playerTransform = PlayerTracker.instance.player.transform;

            // Controllo la distanza da terra per approssimare la posizione originale
            _originalPosition = transform.position;
            if (Physics.Raycast(transform.position, Vector3.down, out var hit, 100, LayerMask.GetMask("Ground")))
            {
                var distanceToGround = hit.distance;
                _originalPosition.y -= distanceToGround;
            }

            // Calcola ora quanto dura la lunghezza della clip di morte
            var clips = animator.runtimeAnimatorController.animationClips;
            foreach (var clip in clips)
            {
                if (clip.name.Contains("Dead"))
                    deadAnimationTime = clip.length;
            }
            
            // range/melee
            if (hasRangedAttacks)
            {
                if (maxDistanceForRangedAttack == 0 || maxDistanceForRangedAttack >= lookRadius)
                    maxDistanceForRangedAttack = lookRadius - 0.1f;
            }
            else
            {
                maxDistanceForRangedAttack = 0;
            }
        }

        protected override void ManageMovement()
        {
            base.ManageMovement();

            // distanza fra il nemico e il player
            var distanceFromPlayer = Vector3.Distance(transform.position, _playerTransform.position);

            // se il nemico vede il player
            if (distanceFromPlayer <= lookRadius
                && _navigationMeshAgent.CalculatePath(_playerTransform.position, _navMeshPath)
                && _navMeshPath.status == NavMeshPathStatus.PathComplete)
            {
                // il nemico viene aggrato
                // se il nemico ha attacchi range lo inizia ad attaccare
                if (hasRangedAttacks && distanceFromPlayer <= maxDistanceForRangedAttack)
                {
                    _navigationMeshAgent.ResetPath();
                }
                // altrimenti si muoverà per poi attaccarlo melee
                else
                {
                    _navigationMeshAgent.SetPath(_navMeshPath);
                    animator.SetBool(IsMoving, true);
                }
            }
            // altrimenti se non lo vede
            else
            {
                // Se è lontano al punto originale
                if (Vector3.Distance(transform.position, _originalPosition) > 3.0f
                    && _navigationMeshAgent.CalculatePath(_originalPosition, _navMeshPath)
                    && _navMeshPath.status == NavMeshPathStatus.PathComplete)
                {
                    // Torna al punto originale
                    animator.SetBool(IsMoving, true);
                    _navigationMeshAgent.SetPath(_navMeshPath);
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
            var distanceFromPlayer = Vector3.Distance(transform.position, _playerTransform.position);

            var rangedCase = hasRangedAttacks && distanceFromPlayer <= maxDistanceForRangedAttack;
            var meleeCase = distanceFromPlayer <= maxDistanceForMeleeAttack;
            // se il nemico è a distanza per colpire con armi bianche
            if (rangedCase || meleeCase)
            {
                // smette di muoversi
                animator.SetBool(IsMoving, false);
                _navigationMeshAgent.ResetPath();

                // se il nemico è a distanza per colpire dalla distanza
                if (meleeCase)
                {
                    animator.ResetTrigger(Distance);
                    animator.SetTrigger(Melee);
                }
                else
                {
                    animator.ResetTrigger(Melee);
                    animator.SetTrigger(Distance);
                }


                // Attacca il player
                animator.SetBool(IsAttacking, true);

                // Ruota verso il player
                FacePlayer();
            }
            // altrimenti non sta attaccando
            else
            {
                animator.ResetTrigger(Melee);
                animator.ResetTrigger(Distance);
                animator.SetBool(IsAttacking, false);
            }
        }

        private void FacePlayer()
        {
            // ricavo la direzione verso il player
            var direction = (_playerTransform.position - transform.position).normalized;
            // trova la rotazione che deve fare per guardare il player
            var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            // applica quella rotazione in maniera più smooth
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        protected override void Die()
        {
            base.Die();
            
            // Distruggi il gameobject
            Destroy(gameObject, deadAnimationTime);
        }

        public override IEnumerator TemporaneousSpeedBuff(int bonusSpeed, int seconds)
        {
            yield return base.TemporaneousSpeedBuff(bonusSpeed, seconds);
            GetComponent<NavMeshAgent>().speed += bonusSpeed;
            yield return Waiter.Active(seconds);
            GetComponent<NavMeshAgent>().speed -= bonusSpeed;
        }

        #region EnemyVisionOnEditor

        private void OnDrawGizmosSelected()
        {
            // Disegna nell'editor una sfera rossa che rappresenterà il volume il cui il nemico può vedere
            var position = transform.position;

            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(position, lookRadius);

            if (hasRangedAttacks)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(position, maxDistanceForRangedAttack);
            }

            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(position, maxDistanceForMeleeAttack);
        }

        #endregion
    }
}