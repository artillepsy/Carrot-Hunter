using UnityEngine;

namespace Core
{
    public class SpriteAnimation : MonoBehaviour
    {
        [SerializeField] private float offsetSpeed = 5f;
        [Space]
        [SerializeField] private float maxOffsetX = 0.10f;
        [SerializeField] private float offsetScaleX = 0.95f;
        [Space]
        [SerializeField] private float maxOffsetY = 0.15f;
        [SerializeField] private float offsetScaleY = 0.95f;
        private Rigidbody2D _rigidbody;
        private float _scaleX, _scaleY;
        private void Awake()
        {
            _rigidbody = GetComponentInParent<Rigidbody2D>();
            _scaleX = transform.localScale.x;
            _scaleY = transform.localScale.y;
        }

        private void Update()
        {
            if(_rigidbody.velocity == Vector2.zero) return;
            
            if (Mathf.Abs(_rigidbody.velocity.y) > Mathf.Abs(_rigidbody.velocity.x))
            {
                var y = _scaleY * Mathf.Abs(Mathf.Sin(Time.time * offsetSpeed) * maxOffsetY) + offsetScaleY;
                transform.localScale = new Vector3(_scaleX, y, 0);
            }
            else
            {
                var x = _scaleX * Mathf.Abs(Mathf.Sin(Time.time * offsetSpeed) * maxOffsetX) + offsetScaleX;
                transform.localScale = new Vector3(x, _scaleY, 0);
            }
            
        }
    }
}