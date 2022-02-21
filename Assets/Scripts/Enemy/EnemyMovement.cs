using Core;
using Player;
using UnityEngine;
using Behaviour = Core.Behaviour;

namespace Enemy
{
    public class EnemyMovement : MonoBehaviour, IOnBehaviourChange
    {
        [Header("Movement")]
        [SerializeField] private float normalSpeed = 1f;
        [SerializeField] private float attackingSpeed = 1.3f;
        [SerializeField] private float dirtySpeed = 0.4f;
        [Header("Pathfinding")]
        [SerializeField] private float nextTargetDistance = 0.1f;
        [SerializeField] private float checkPathRateInSeconds = 0.3f;
        
        private float _timeSincePathUpdate = 0f;
        private float _currentSpeed;
        private Transform _target;
        private Transform _player;
        private Navigation _navigation;
        private Vector2 _mainDirection, _secondDirection;
        private Vector2 _nextDotPosition;
        private Rigidbody2D _rb;

        public void OnBehaviourChange(Behaviour newBehaviour)
        {
            switch (newBehaviour)
            {
                case Behaviour.Normal:
                    _currentSpeed = normalSpeed;
                    ChangeTarget();
                    break;
                case Behaviour.Attacking:
                    _currentSpeed = attackingSpeed;
                    _target = _player;
                    break;
                case Behaviour.Dirty:
                    _currentSpeed = dirtySpeed;
                    ChangeTarget();
                    break;
            }
        }
        
        private void Start()
        {
            _currentSpeed = normalSpeed;
            _navigation = GetComponent<Navigation>();
            _rb = GetComponent<Rigidbody2D>();
            _player = FindObjectOfType<PlayerMovement>().transform;
            ChangeTarget();
            _player.GetComponent<CarrotPicker>().OnCarrotPickUp.AddListener(ChangeTarget);
        }

        private void Update()
        {
            if (IsTimeToUpdatePath()) UpdatePath();
            if (_navigation.TryToMove(_mainDirection, _currentSpeed)) return;
            if (_navigation.TryToMove(_secondDirection, _currentSpeed)) return;
             _nextDotPosition = _navigation.GetNearestDot(_navigation.OverlapCircleRadius);
            //_rb.velocity = Vector2.zero;
        }

        private bool IsTimeToUpdatePath()
        {
            if (_timeSincePathUpdate >= checkPathRateInSeconds)
            {
                _timeSincePathUpdate = 0f;
                return true;
            }
            _timeSincePathUpdate += Time.deltaTime;
            return false;
        }
        private void UpdatePath()
        {
            if (_target == null) ChangeTarget();
            else if (_target.CompareTag("Carrot"))
            {
                CheckDistanceToTarget();
            }
            if (_nextDotPosition == _navigation.NextDotPosition) return;
            _nextDotPosition = _navigation.NextDotPosition;
            
            var direction = _target.position - transform.position;
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                _mainDirection = new Vector2(direction.x, 0);
                _secondDirection = new Vector2(0, direction.y);
            }
            else
            {
                _mainDirection = new Vector2(0, direction.y);
                _secondDirection = new Vector2(direction.x, 0);
            }
        }
        private void ChangeTarget()
        {
            var carrots = GameObject.FindGameObjectsWithTag("Carrot");
            if (carrots.Length == 0)
            {
                _target = _player;
                return;
            }
            _target = carrots[Random.Range(0, carrots.Length)].transform;
        }
        private void CheckDistanceToTarget()
        {
            var distance = (transform.position - _target.position).magnitude;
            if (distance > nextTargetDistance) return;
            ChangeTarget();
        }
    }
}