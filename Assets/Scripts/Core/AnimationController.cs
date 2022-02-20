using Enemy;
using UnityEngine;

namespace Core
{
    public class AnimationController : MonoBehaviour, IOnStateChange
    {
        [SerializeField] private float minVelocityToChange = 0.1f;
        [SerializeField] private float minAngleToChange = 10f;
        
        private float _sqrMinVelocityToChange;
        private Animator _animator;
        private WalkState _walkState;
        private Rigidbody2D _rb;
        private Vector2 _prevDirection;
        private State _currentState;
        
        private void Awake()
        {
            _prevDirection = Vector2.up;
            _rb = GetComponentInParent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _sqrMinVelocityToChange = minVelocityToChange * minVelocityToChange;
            _currentState = State.Normal;
            _walkState = WalkState.WalkDown;
        }
        
        private void Update()
        {
            if (_rb.velocity.sqrMagnitude < _sqrMinVelocityToChange) return;
            ChangeWalkState(_rb.velocity);
        }
        
        private void ChangeWalkState(Vector2 direction)
        {
            var prevAngle = Vector2.Angle(_prevDirection, direction);
            if (prevAngle < minAngleToChange) return;
            
            var angle = Vector2.Angle(Vector2.up, direction);

            if (angle < 45)
            {
                _walkState = WalkState.WalkUp;
            }
            else if (angle < 135)
            {
                _walkState = direction.x > 0 ? WalkState.WalkRight : WalkState.WalkLeft;
            }
            else
            {
                _walkState = WalkState.WalkDown;
            }

            _prevDirection = direction;
            _animator.SetInteger("State", (int) _walkState + (int) _currentState);
            
        }

        public void OnStateChange(State newState)
        {
            _currentState = newState;
            _animator.SetInteger("State", (int) _walkState + (int) _currentState);
            Debug.Log((int) _walkState + (int) _currentState);
            Debug.Log(_currentState);
            
        }
    }
}