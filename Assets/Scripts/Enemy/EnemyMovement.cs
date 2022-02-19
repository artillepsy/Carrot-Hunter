using System.Collections;
using System.Collections.Generic;
using Core;
using Player;
using UnityEngine;

namespace Enemy
{
    public class EnemyMovement : AbstractMovement, IOnEnemyStateChange
    {
        [SerializeField] private Transform target;
        [SerializeField] private float checkPathRateInSeconds = 0.3f;
        
        private Coroutine _updPathCoroutine;
        private List<Vector2> cachedDots;
        private bool _isFollowing = false;

        public void OnStateChange(EnemyState newState)
        {
            if (newState != EnemyState.MovingToTarget)
            {
                StopAllCoroutines();
                _rb.velocity = Vector2.zero;
                _isFollowing = false;
                _updPathCoroutine = null;
            }
            else
            {
                _isFollowing = true;
                if (_updPathCoroutine != null) return;
                _updPathCoroutine = StartCoroutine(UpdatePathCoroutine());
            }
        }
        
        private void Start()
        {
            cachedDots = new List<Vector2>();
            target = FindObjectOfType<PlayerMovement>().transform;
        }
        
        private void Update()
        {
            if (!_isFollowing) return;
            Movement();
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
                var direction = target.position - transform.position;
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