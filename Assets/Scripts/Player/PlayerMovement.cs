using System.Collections;
using Core;
using Enemy;
using UnityEngine;
using Behaviour = Core.Behaviour;

namespace Player
{
    public class PlayerMovement : MonoBehaviour, IOnBehaviourChange
    {
        [Header("Input")]
        [SerializeField] private Joystick joystick;
        [Range(0, 1)] 
        [SerializeField] private float startWalkJoystickValue = 0.15f;
        
        [Header("Movement")]
        [SerializeField] private float normalSpeed = 2f;
        
        [Header("OnDamaged")]
        [SerializeField] private float onDamagedSpeed = 3f;

        private Rigidbody2D _rb;
        private Navigation _navigation;
        private Coroutine _onDamagedCoroutine;
        private float _currentSpeed;
        
        public void OnBehaviourChange(Behaviour newBehaviour)
        {
            if (newBehaviour == Behaviour.Damaged) _currentSpeed = onDamagedSpeed;
            else _currentSpeed = normalSpeed;
        }
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _currentSpeed = normalSpeed;
            _navigation = GetComponent<Navigation>();
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
            if (direction.magnitude < startWalkJoystickValue)
            {
                _rb.velocity = Vector2.zero;
                return;
            }
            if (!_navigation.TryToMove(direction, _currentSpeed))
            {
                _rb.velocity = Vector2.zero;
            }
        }
    }
}
