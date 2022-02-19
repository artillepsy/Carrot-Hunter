using Characters;
using Core;
using UnityEngine;
namespace Player
{
    public class PlayerMovement : AbstractMovement
    {
        [SerializeField] private Joystick joystick;
        [Range(0, 1)] 
        [SerializeField] private float startWalkValue = 0.3f;

        private float _sqrStartWalkValue;
        private WalkState _walkState;
        private void Awake()
        {
            base.Awake();
            _walkState = WalkState.WalkDown;
            _sqrStartWalkValue = startWalkValue * startWalkValue;
        }

        private void Update()
        {
            MovementInput();
        }

        private void MovementInput()
        {
            var xAxis = joystick.Horizontal;
            var yAxis = joystick.Vertical;
            if (Mathf.Abs(xAxis) < Mathf.Abs(yAxis)) xAxis = 0;
            else yAxis = 0;
            var direction = new Vector2(xAxis, yAxis);
            if (direction.sqrMagnitude < _sqrStartWalkValue)
            {
                _rb.velocity = Vector2.zero;
                return;
            }
            SetCurrentDot(GetDots(direction));
            Movement();
        }
    }
}
