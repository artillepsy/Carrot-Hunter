using UnityEngine;

namespace Core
{
    public class Navigation : MonoBehaviour
    {
        [SerializeField] private bool drawGizmos = false;
        [Space]
        [SerializeField] private float overlapCircleDistance = 0.92f;
        [SerializeField] private float overlapCircleRadius = 0.35f;
        [SerializeField] private float minDistanceToNextDot = 0.1f;
        
        private Rigidbody2D _rb;
        private Vector3 _nextDotPosition;
        private Vector2 _prevDirection;

        public bool NextDotIsFar => _distanceToNextDot > minDistanceToNextDot;
        private float _distanceToNextDot => (_nextDotPosition - transform.position).magnitude;
        private Vector2 _directionToNextDot => _nextDotPosition - transform.position;
        public Vector2 NextDotPosition => _nextDotPosition;
        public float OverlapCircleRadius => overlapCircleRadius;
        
        public bool TryToMove(Vector2 direction, float speed)
        {
            var directionEqual = direction == _prevDirection;
            _prevDirection = direction;
            if (!directionEqual || !NextDotIsFar)
            {
                var circleCenter = (Vector2) transform.position + direction.normalized * overlapCircleDistance;
                if (!TryGetDotPosition(circleCenter, out var dotPosition))
                {
                    return false;
                }
                _nextDotPosition = dotPosition;
            }
            _rb.velocity = Vector2.ClampMagnitude(_directionToNextDot.normalized * speed, speed);
            return true;
        }
        
        public Vector2 GetNearestDot(float radius)
        {
            var minDistance = float.PositiveInfinity;
            var dotPosition = transform.position;
            
            foreach (var coll in Physics2D.OverlapCircleAll(transform.position, radius))
            {
                if(!coll.CompareTag("Dot")) continue;
                var distance = (transform.position - coll.transform.position).sqrMagnitude;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    dotPosition = coll.transform.position;
                }
            }
            return dotPosition;
        }
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _nextDotPosition = GetNearestDot(overlapCircleRadius);
            _prevDirection = Vector2.zero;
        }
        
        private bool TryGetDotPosition(Vector2 overlapCenter, out Vector3 result)
        {
            result = Vector2.zero;
            var minDistance = float.PositiveInfinity;
            var colliders = Physics2D.OverlapCircleAll(overlapCenter, overlapCircleRadius);
            //Debug.DrawLine(overlapCenter, overlapCenter + Vector2.right*overlapCircleRadius, Color.green, 3);
            var dotExists = false;
            foreach (var other in colliders)
            {
                if(!other.CompareTag("Dot")) continue;
                var distance = (overlapCenter - (Vector2)transform.position).magnitude;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    result = other.transform.position;
                    dotExists = true;
                }
            }
            return dotExists;
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