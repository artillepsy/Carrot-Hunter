using System.Collections;
using System.Collections.Generic;
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
                    ChangeTarget(null);
                    break;
                
                case State.Attacking:
                    SetSpeedMultiplier(attackingSpeedMultiplier);
                    ChangeTarget(_player);
                    break;
                
                case State.Dirty:
                    SetSpeedMultiplier(dirtySpeedMultiplier);
                    ChangeTarget(null);
                    break;
            }
        }
        
        private void Start()
        {
            _player = FindObjectOfType<PlayerMovement>().transform;
            cachedDots = new List<Vector2>();
            _target = _player;
            StartCoroutine(UpdatePathCoroutine());
        }
        
        private void Update()
        {
            Movement();
        }

        private void ChangeTarget(Transform newTarget)
        {
            // if attacking, target - player
            // else target - carrot
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