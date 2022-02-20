using System.Collections;
using Core;
using UnityEngine;
namespace Player
{
    public class PlayerMovement : AbstractMovement
    {
        [SerializeField] private Joystick joystick;
        [Range(0, 1)] 
        [SerializeField] private float startWalkJoystickValue = 0.3f;
        [Header("On Take Damage Settings")]
        [SerializeField] private float damagedSpeedMultiplier = 1.5f;
        [SerializeField] private float damagedTimeInSeconds = 2f;

        private float _sqrStartWalkValue;
        private WalkState _walkState;
        private Coroutine _onDamagedCoroutine;
        private float _time;
        private void Awake()
        {
            base.Awake();
            _time = 0;
            _walkState = WalkState.WalkDown;
            _sqrStartWalkValue = startWalkJoystickValue * startWalkJoystickValue;
            GetComponent<PlayerHealth>().OnTakeDamage.AddListener(OnTakeDamage);
        }

        private void Update()
        {
            MovementInput();
        }

        private void OnTakeDamage(int health)
        {
            _time = damagedTimeInSeconds;
            if (_onDamagedCoroutine == null)
            {
                StartCoroutine(MoveFasterCoroutine());
            }
        }
        private IEnumerator MoveFasterCoroutine()
        {
            SetSpeedMultiplier(damagedSpeedMultiplier);
            while (_time > 0)
            {
                _time -= Time.deltaTime;
                yield return null;
            }
            SetSpeedMultiplier(1);
            _onDamagedCoroutine = null;
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
