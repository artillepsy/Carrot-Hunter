using Core;
using Enemy;
using UnityEngine;
using UnityEngine.Events;
using Behaviour = Core.Behaviour;

namespace Player
{
    public class PlayerHealth : MonoBehaviour, IOnBehaviourChange
    {
        public readonly UnityEvent OnTakeDamage = new UnityEvent();
        [SerializeField] private int health = 3;
        [SerializeField] private ParticleSystem onDamageParticleSystem;
        
        public int Health => health;

        public void OnBehaviourChange(Behaviour newBehaviour)
        {
            if (newBehaviour != Behaviour.Damaged) return;
            health--;
            OnTakeDamage?.Invoke();
            Instantiate(onDamageParticleSystem, transform.position, Quaternion.identity, transform).Play();
        }
    }
}