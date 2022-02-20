using System.Collections.Generic;
using Player;
using UnityEngine;

namespace UI
{
    public class HudCanvas : MonoBehaviour
    {
        [SerializeField] private Transform heartPrefab;
        [SerializeField] private Transform heartsParent;
        
        private PlayerHealth _playerHealth;
        private List<Transform> _hearts;
        
        private void Start()
        {
            _hearts = new List<Transform>();
            _playerHealth = FindObjectOfType<PlayerHealth>();
            _playerHealth.OnHealthChange.AddListener(ChangeHealth);
            SetUpHealth();
        }

        private void SetUpHealth()
        {
            for (int i = 0; i < _playerHealth.Health; i++)
            {
                _hearts.Add(Instantiate(heartPrefab, heartsParent));
            }
        }
        private void ChangeHealth(int health)
        {
            if (_hearts.Count == 0) return;
            var heart = _hearts[0];
            _hearts.Remove(heart);
            Destroy(heart.gameObject);
        }
    }
}