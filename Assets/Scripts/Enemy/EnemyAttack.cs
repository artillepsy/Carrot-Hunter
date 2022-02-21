using Core;
using Player;
using UnityEngine;
using Behaviour = Core.Behaviour;

namespace Enemy
{
    public class EnemyAttack : MonoBehaviour, IOnBehaviourChange
    {
        [SerializeField] private bool drawGizmos;
        [SerializeField] private float kickReloadInSeconds = 1.5f;
        [SerializeField] private float kickRadius;

        private IAffectOnDamage _damageTarget;
        private Transform _player;
        private bool _attackMode = false;
        private bool _reload = false;

        public void OnBehaviourChange(Behaviour newBehaviour)
        {
            _attackMode = newBehaviour == Behaviour.Attacking;
        }

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player").transform;
            _damageTarget = _player.GetComponent<IAffectOnDamage>();
        }

        private void Update()
        {
            if (_reload || !_attackMode) return;
            if (!PlayerIsNear()) return;
            _damageTarget.OnDamaged();
            _reload = true;
            Invoke(nameof(FinishReload), kickReloadInSeconds);
        }

        private bool PlayerIsNear() => Vector2.Distance(_player.position, transform.position) < kickRadius;

        private void FinishReload()
        {
            _reload = false;
        }
        
        private void OnDrawGizmosSelected()
        {
            if (!drawGizmos) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, kickRadius);
        }
    }
}