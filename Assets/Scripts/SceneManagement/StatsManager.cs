using System.Collections.Generic;
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
        [SerializeField] private GameObject finishArrow;
        public readonly UnityEvent OnGameOver = new UnityEvent(); 
        public readonly UnityEvent OnWin = new UnityEvent(); 
        private int _startCarrotCount = 0;
        private int _endCarrotCount;
        private int _pickedCarrots = 0;
        private PlayerHealth _playerHealth;
        private List<Transform> _hearts;
        
        private bool _carrotsCollected = false;
        private bool _playerInTrigger = false;
        private bool _endGame = false;

        public int CalculateScore(int maxValue)
        {
            var score = (float)_endCarrotCount / _startCarrotCount;
            Debug.Log(score);
            var segment = 1f / maxValue;
            Debug.Log(segment);
            Debug.Log(Mathf.RoundToInt(score / segment));
            return Mathf.RoundToInt(score / segment);
        }

        public void SetStartCarrotCount(int count)
        {
            _startCarrotCount = count;  
            _endCarrotCount = count;
            scoreText.text = "0 / " + _endCarrotCount;
        } 
        private void Start()
        {
            finishArrow.SetActive(false);
            
            _hearts = new List<Transform>();
            _playerHealth = FindObjectOfType<PlayerHealth>();
            _playerHealth.OnTakeDamage.AddListener(ChangeHeartsCount);

            Bomb.OnCarrotExplose.AddListener(DecrementCarrotCount);
            FindObjectOfType<CarrotPicker>().OnCarrotPickUp.AddListener(IncreaseScore);
            DisplayHearts();
        }
        
        private void Update()
        {
            if (_playerInTrigger && _carrotsCollected && !_endGame)
            {
                OnWin?.Invoke();
                _endGame = true;
            }
        }

        private void DecrementCarrotCount()
        {
            _endCarrotCount--;
            if (_pickedCarrots != _endCarrotCount) return;
            if (_pickedCarrots == 0)
            {
                OnGameOver?.Invoke();
                _endGame = true;
            }
            finishArrow.SetActive(true);
            _carrotsCollected = true;
        }
        private void DisplayHearts()
        {
            for (int i = 0; i < _playerHealth.Health; i++)
            {
                _hearts.Add(Instantiate(heartPrefab, heartsParent));
            }
        }
        private void ChangeHeartsCount(int health)
        {
            if (health == 0)
            {
                OnGameOver?.Invoke();
                _endGame = true;
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
            if (_pickedCarrots != _endCarrotCount) return;
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