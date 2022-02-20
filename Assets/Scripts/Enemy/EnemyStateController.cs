using System.Collections;
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

        private IconController _iconController;
        private float _sqrDistanceToMoving;
        private float _sqrDistanceToAttacking;
        private Transform _player;
        private State _currentState;
        private State _prevState;
        private AnimationController _animationController;
        private List<IOnStateChange> _components;
        private float _dirtyTime = 0;

        public void MakeDirty(float dirtyTimeInSeconds)
        {
            _dirtyTime = dirtyTimeInSeconds;
        }
        
        private void Awake()
        {
            _components = GetComponents<IOnStateChange>().ToList();
            _components.Add(GetComponentInChildren<AnimationController>());
            _iconController = GetComponentInChildren<IconController>();
            _currentState = State.Normal;
            _prevState = _currentState;
            _sqrDistanceToAttacking = distanceToAttacking * distanceToAttacking;
        }

        private void Start()
        {
            _player = FindObjectOfType<PlayerMovement>().transform;
            StartCoroutine(ControlStateCoroutine());
        }

        private IEnumerator ControlStateCoroutine()
        {
            while (true)
            {
                yield return null;
                if (_dirtyTime > 0)
                {
                    _currentState = State.Dirty;
                    _iconController.SetIcon(Icons.Dirty);
                    ChangeStateInComponents();
                    while (_dirtyTime > 0)
                    {
                        _dirtyTime -= Time.deltaTime;
                        yield return null;
                    }
                }
                
                var sqrDistance = (transform.position - _player.position).sqrMagnitude;
                if (sqrDistance > _sqrDistanceToAttacking)
                {
                    _currentState = State.Normal;
                    _iconController.SetIcon(Icons.Normal);
                }
                else
                {
                    _currentState = State.Attacking;
                    _iconController.SetIcon(Icons.Attackig);
                }
   
                if (_prevState == _currentState) continue;
                ChangeStateInComponents();

            }
        }

        private void ChangeStateInComponents()
        {
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