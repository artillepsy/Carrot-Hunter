using System;
using UnityEngine;

namespace SceneManagement
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private GameObject finishArrow;
        private bool _carrotsCollected = false;
        private bool _playerInTrigger = false;
        private void Start()
        {
            finishArrow.SetActive(false);
            var hudCanvas = FindObjectOfType<StatsManager>();
            hudCanvas.OnCarrotsCollected.AddListener(OnCarrotsCollected);
            hudCanvas.OnPlayerDie.AddListener(OnPlayerDie);
        }

        private void Update()
        {
            if (_playerInTrigger && _carrotsCollected)
            {
                Debug.Log("win");
            }
        }

        private void OnPlayerDie()
        {
            
        }

        private void OnCarrotsCollected()
        {
            finishArrow.SetActive(true);
            _carrotsCollected = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.isTrigger && other.CompareTag("Player"))
            {
                _playerInTrigger = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.isTrigger && other.CompareTag("Player"))
            {
                _playerInTrigger = false;
            }
        }
    }
}