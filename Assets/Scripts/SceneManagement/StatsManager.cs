﻿using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace SceneManagement
{
    public class StatsManager : MonoBehaviour
    {
        [SerializeField] private Transform heartPrefab;
        [SerializeField] private Transform heartsParent;
        [SerializeField] private TextMeshProUGUI scoreText;
        public readonly UnityEvent OnPlayerDie = new UnityEvent(); 
        public readonly UnityEvent OnCarrotsCollected = new UnityEvent(); 
        private int _startCarrotCount = 0;
        private int _carrotsOnScene;
        private int _pickedCarrots = 0;
        private PlayerHealth _playerHealth;
        private List<Transform> _hearts;
        
        private void Start()
        {
            _hearts = new List<Transform>();
            _playerHealth = FindObjectOfType<PlayerHealth>();
            _playerHealth.OnTakeDamage.AddListener(ChangeHealth);
            
            _startCarrotCount = GameObject.FindGameObjectsWithTag("Carrot").Length;
            _carrotsOnScene = _startCarrotCount;
            FindObjectOfType<CarrotPicker>().OnCarrotPickUp.AddListener(IncreaseScore);
            Bomb.OnCarrotExplose.AddListener(()=> _carrotsOnScene--);
            scoreText.text = "0 / " + _carrotsOnScene;
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
            if (health == 0)
            {
                OnPlayerDie?.Invoke();
                return;
            }
            var heart = _hearts[0];
            _hearts.Remove(heart);
            Destroy(heart.gameObject);
        }
        
        private void IncreaseScore()
        {
            _pickedCarrots++;
            scoreText.text = _pickedCarrots + " / " + _startCarrotCount;
            if (_pickedCarrots == _carrotsOnScene)
            {
                OnCarrotsCollected?.Invoke();
            }
        }
    }
}