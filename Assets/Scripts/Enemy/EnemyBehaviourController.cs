using Core;
using Player;
using UnityEngine;
using Behaviour = Core.Behaviour;

namespace Enemy
{
    public class EnemyBehaviourController : BaseBehaviourController, IAffectOnDamage
    {
        [SerializeField] private bool showGizmos = false;
        [SerializeField] private float distanceToAttackBehaviour;
        [SerializeField] private float dirtyTimeInSeconds = 5f;
        private Transform _player;
        private float _dirtyTime = 0;
        
        public void OnDamaged()
        {
            _dirtyTime = dirtyTimeInSeconds;
            _currentBehaviour = Behaviour.Dirty;
        }

        private void Start()
        {
            _player = FindObjectOfType<PlayerMovement>().transform;
        }

        private void Update()
        {
            UpdateBehaviour();
        }

        protected override void UpdateBehaviour()
        {
            if (_dirtyTime > 0)
            {
                NotifyTargets();
                _dirtyTime -= Time.deltaTime;
                return;
            }
            var distance = (transform.position - _player.position).magnitude;
            if (distance > distanceToAttackBehaviour)
            {
                _currentBehaviour = Behaviour.Normal;
            }
            else
            {
                _currentBehaviour = Behaviour.Attacking;
            }
            NotifyTargets();
        }

        private void OnDrawGizmosSelected()
        {
            if (!showGizmos) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, distanceToAttackBehaviour);
        }

    }
}