using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using Player;
using UnityEngine;

namespace Enemy
{
    public class EnemyMovement : AbstractMovement, IOnStateChange
    {
        
        [SerializeField] private float checkPathRateInSeconds = 0.3f;
        [Header("Speed settings")]
        [SerializeField] private float attackingSpeedMultiplier = 1.3f;
        [SerializeField] private float dirtySpeedMultiplier = 0.4f;
        [SerializeField] private float nextCarrotDistance = 0.1f;

        private float _sqrNextCarrotDistance;
        private Transform _target;
        private Transform _player;
        private Coroutine _updPathCoroutine;
        private List<Vector2> cachedDots;

        public void OnStateChange(State newState)
        {
            switch (newState)
            {
                case State.Normal:
                    SetSpeedMultiplier(1);
                    ChangeTargetToCarrot();
                    break;
                
                case State.Attacking:
                    SetSpeedMultiplier(attackingSpeedMultiplier);
                    _target = _player;
                    break;
                
                case State.Dirty:
                    SetSpeedMultiplier(dirtySpeedMultiplier);
                    ChangeTargetToCarrot();
                    break;
            }
        }
        
        private void Start()
        {
            _sqrNextCarrotDistance = nextCarrotDistance * nextCarrotDistance;
            _player = FindObjectOfType<PlayerMovement>().transform;
            _player.GetComponent<CarrotPicker>().OnCarrotPickUp.AddListener(ChangeTargetToCarrot);
            cachedDots = new List<Vector2>();
            _target = _player;
            StartCoroutine(UpdatePathCoroutine());
        }
        
        private void Update()
        {
            Movement();
        }
        
        private void ChangeTargetToCarrot()
        {
            var carrots = GameObject.FindGameObjectsWithTag("Carrot").ToList();
            if (carrots.Count == 0)
            {
                _target = _player;
                return;
            }
            _target = carrots[Random.Range(0, carrots.Count)].transform;
        }

        private void CheckDistanceToCarrot()
        {
            if (!_target.CompareTag("Carrot")) return;
            var sqrDistance = (transform.position - _target.position).sqrMagnitude;
            if (sqrDistance > _sqrNextCarrotDistance) return;
            ChangeTargetToCarrot();
        }
        private IEnumerator UpdatePathCoroutine()
        {
            while (true)
            {
                if (!_reachedDot)
                {
                    yield return null;
                    continue;
                }

                if (_target != null)
                {
                    CheckDistanceToCarrot();
                }
                else
                {
                    ChangeTargetToCarrot();
                }
                
                var direction = _target.position - transform.position;
                Vector2 mainVect, secondVect;
                
                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                {
                    mainVect = new Vector2(direction.x, 0);
                    secondVect = new Vector2(0, direction.y);
                }
                else
                {
                    mainVect = new Vector2(0, direction.y);
                    secondVect = new Vector2(direction.x, 0);
                }

                cachedDots = GetDots(mainVect);
                if (cachedDots.Count == 0)
                {
                    cachedDots = GetDots(secondVect);
                }
                SetCurrentDot(cachedDots);
                yield return new WaitForSeconds(checkPathRateInSeconds);
            }
        }
    }
}