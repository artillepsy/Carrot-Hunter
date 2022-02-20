using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class PlayerHealth : MonoBehaviour
    {
        public readonly UnityEvent<int> OnTakeDamage = new UnityEvent<int>();
        [SerializeField] private int health = 3;
        [SerializeField] private ParticleSystem onDamageParticleSystem;
        
        public int Health => health;
        public void Damage()
        {
            health--;
            OnTakeDamage?.Invoke(health);
            Instantiate(onDamageParticleSystem, transform.position, Quaternion.identity, transform).Play();
        }
    }
}