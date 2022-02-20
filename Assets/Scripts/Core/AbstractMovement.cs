using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public abstract class AbstractMovement : MonoBehaviour
    {
        [SerializeField] private bool drawGizmos = false;
        [SerializeField] private float speed = 5f;
        [SerializeField] private float overlapCircleDistance = 1f;
        [SerializeField] private float overlapCircleRadius = 0.35f;
        [SerializeField] private float stoppingDistance = 0.1f;
        private float _sqrStoppingDistance;
        private Vector2 _currentDot;
        protected Rigidbody2D _rb;
        protected bool _reachedDot = false;
        private float _finalSpeed;

        protected void SetSpeedMultiplier(float newMultiplier)
        {
            _finalSpeed = speed * newMultiplier;
        }
        protected void Awake()
        {
            _finalSpeed = speed;
            _currentDot = Vector2.zero;
            _rb = GetComponent<Rigidbody2D>();
            _sqrStoppingDistance = stoppingDistance * stoppingDistance;
        }
        protected void Movement()
        {
            var directionToDot = _currentDot - (Vector2) transform.position;
            _reachedDot = false;
            if (directionToDot.sqrMagnitude < _sqrStoppingDistance)
            {
                _reachedDot = true;
                _rb.velocity = Vector2.zero;
                return;
            }
            _rb.velocity = Vector2.ClampMagnitude(directionToDot.normalized * _finalSpeed, _finalSpeed);
        }

        protected void SetCurrentDot(List<Vector2> dots)
        {
            if (dots.Count != 0)
            {
                float maxDistance = 0;
                foreach (var dot in dots)
                {
                    var distance = (dot - (Vector2) transform.position).sqrMagnitude;
                    if (distance <= maxDistance) continue;
                    maxDistance = distance;
                    _currentDot = dot;
                }
            }
        }
        protected List<Vector2> GetDots(Vector2 direction)
        {
            var overlapCircleCenter = (Vector2) transform.position + direction.normalized * overlapCircleDistance;
            var colliders = Physics2D.OverlapCircleAll(overlapCircleCenter, overlapCircleRadius);
            Debug.DrawLine(overlapCircleCenter, overlapCircleCenter + Vector2.right*overlapCircleRadius, Color.green, 3);
            var points = new List<Vector2>();
            foreach (var other in colliders)
            {
                if(!other.CompareTag("Dot")) continue;
                points.Add(other.transform.position);
            }
            return points;
        }
        private void OnDrawGizmosSelected()
        {
            if (!drawGizmos) return;
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, overlapCircleDistance);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, overlapCircleRadius);
        }
    }
}