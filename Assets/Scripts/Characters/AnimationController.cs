using UnityEngine;

namespace Characters
{
    public enum WalkState
    {
        WalkUp,
        AttackingWalkUp,
        DirtyWalkUp,
        WalkDown,
        AttackingWalkDown,
        DirtyWalkDown,
        WalkRight,
        AttackingWalkRight,
        DirtyWalkRight,
        WalkLeft,
        AttackingWalkLeft,
        DirtyWalkLeft
    }
    public enum State
    {
        Normal,
        Attacking,
        Dirty,
    }

    public class AnimationController : MonoBehaviour
    {
        [SerializeField] private float minVelocityToChange = 0.1f;
        [SerializeField] private float minAngleToChange = 10f;
        
        private float _sqrMinVelocityToChange;
        private Animator _animator;
        private WalkState _walkState;
        private Rigidbody2D _rb;
        private Vector2 _prevDirection;
        public State CurrentState { get; set; }

        private void Awake()
        {
            _prevDirection = Vector2.up;
            _rb = GetComponentInParent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _sqrMinVelocityToChange = minVelocityToChange * minVelocityToChange;
            CurrentState = State.Normal;
            _walkState = WalkState.WalkDown;
        }
        
        private void Update()
        {
            if (_rb.velocity.sqrMagnitude < _sqrMinVelocityToChange) return;
            ChangePlayerWalkState(_rb.velocity);
        }
        
        private void ChangePlayerWalkState(Vector2 direction)
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
            _animator.SetInteger("State", (int) _walkState + (int) CurrentState);
        }

    }
}