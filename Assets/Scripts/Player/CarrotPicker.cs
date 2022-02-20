using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class CarrotPicker : MonoBehaviour
    {
        public readonly UnityEvent OnCarrotPickUp = new UnityEvent();
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Carrot")) return;
            OnCarrotPickUp?.Invoke();
            Destroy(other.gameObject);
        }
    }
}