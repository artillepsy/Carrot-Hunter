using UnityEngine;

namespace Core
{
    public class AnimationController : MonoBehaviour, IOnBehaviourChange
    {
        [SerializeField] private float minVelocityToChange = 0.1f;
        [SerializeField] private Animator animator;
        
        private WalkDirection walkDirection;
        private Rigidbody2D _rb;
        private static readonly int _walkDirectionId = Animator.StringToHash("WalkDirection");
        private static readonly int _behaviourId = Animator.StringToHash("Behaviour");

        public void OnBehaviourChange(Behaviour newBehaviour)
        {
            animator.SetInteger(_behaviourId,  (int) newBehaviour);
        }
        
        private void Awake()
        {
            _rb = GetComponentInParent<Rigidbody2D>();
            animator.SetInteger(_behaviourId,  (int) Behaviour.Normal);
            animator.SetInteger(_walkDirectionId,  (int) WalkDirection.Down);
        }
        
        private void Update()
        {
            UpdateDirection();
        }
        
        private void UpdateDirection()
        {
            if (_rb.velocity.magnitude < minVelocityToChange) return;
            var currentDirection = WalkDirection.Down;
            Vector2 direction;
            if (Mathf.Abs(_rb.velocity.x) > Mathf.Abs(_rb.velocity.y))
            {
                direction = new Vector2(_rb.velocity.x, 0).normalized;
            }
            else
            {
                direction = new Vector2(0, _rb.velocity.y).normalized;
            }

            if (direction == Vector2.up) currentDirection = WalkDirection.Up;
            else if (direction == Vector2.down) currentDirection = WalkDirection.Down;
            else if (direction == Vector2.left) currentDirection = WalkDirection.Left;
            else if (direction == Vector2.right) currentDirection = WalkDirection.Right;
            
            animator.SetInteger(_walkDirectionId,  (int) currentDirection);
        }
    }
}