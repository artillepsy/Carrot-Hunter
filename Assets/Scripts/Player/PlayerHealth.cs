using UnityEngine;

namespace Player
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private int health = 3;
        
        public void Decrement() => health--;
    }
}