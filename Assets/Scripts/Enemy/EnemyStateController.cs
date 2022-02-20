using System.Collections.Generic;
using System.Linq;
using Core;
using Player;
using UnityEngine;

namespace Enemy
{
    public class EnemyStateController : MonoBehaviour
    {
        [SerializeField] private bool showGizmos = false;
        [SerializeField] private float distanceToAttacking;
        

        private float _sqrDistanceToMoving;
        private float _sqrDistanceToAttacking;
        private Transform _player;
        private State _currentState;
        private State _prevState;
        private AnimationController _animationController;
        private List<IOnStateChange> _components;
        private void Awake()
        {
            _components = GetComponents<IOnStateChange>().ToList();
            _components.Add(GetComponentInChildren<AnimationController>());
            
            _currentState = State.Normal;
            _prevState = _currentState;
            _sqrDistanceToAttacking = distanceToAttacking * distanceToAttacking;
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
            if (sqrDistance > _sqrDistanceToAttacking)
            {
                _currentState = State.Normal;
            }
            else
            {
                _currentState = State.Attacking;
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
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, distanceToAttacking);
        }
    }
}