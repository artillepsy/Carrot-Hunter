using Player;
using UnityEngine;

namespace Enemy
{
    public class EnemyStateController : MonoBehaviour
    {
        [SerializeField] private bool showGizmos = false;
        [SerializeField] private float distanceToMoving;
        [SerializeField] private float distanceToAttack;

        private float _sqrDistanceToMoving;
        private float _sqrDistanceToAttack;
        private Transform _player;
        private EnemyState _currentState;
        private EnemyState _prevState;
        private IOnEnemyStateChange[] _components;
        private void Awake()
        {
            _components = GetComponents<IOnEnemyStateChange>();
            _currentState = EnemyState.WalkingAround;
            _prevState = _currentState;
            _sqrDistanceToMoving = distanceToMoving * distanceToMoving;
            _sqrDistanceToAttack = distanceToAttack * distanceToAttack;
        }

        private void Start()
        {
            _player = FindObjectOfType<PlayerMovement>().transform;
        }

        private void Update()
        {
            CheckDistanceToPlayer();
        }

        private void CheckDistanceToPlayer()
        {
            var sqrDistance = (transform.position - _player.position).sqrMagnitude;
            if (sqrDistance < _sqrDistanceToAttack)
            {
                _currentState = EnemyState.Attacking;
            }
            else if (sqrDistance < _sqrDistanceToMoving)
            {
                _currentState = EnemyState.MovingToTarget;
            }
            else
            {
                _currentState = EnemyState.WalkingAround;
            }
            if (_prevState == _currentState) return;
            
            foreach (var component in _components)
            {
                component.OnStateChange(_currentState);
            }
            _prevState = _currentState;
        }

        private void OnDrawGizmosSelected()
        {
            if (!showGizmos) return;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, distanceToMoving);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, distanceToAttack);
        }
    }
}