﻿using System.Collections;
using Core;
using Player;
using UnityEngine;

namespace Enemy
{
    public class EnemyAttacking : MonoBehaviour, IOnStateChange
    {
        [SerializeField] private float attackRateInSeconds = 1.5f;
        private PlayerHealth _playerHealth;
        private bool _isAttacking = false;
        private bool _playerIsNear = false;
        
        public void OnStateChange(State newState)
        {
            _isAttacking = newState == State.Attacking;
        }

        private void Start()
        {
            _playerHealth = FindObjectOfType<PlayerHealth>();
            StartCoroutine(AttackingCoroutine());
        }

        private IEnumerator AttackingCoroutine()
        {
            while (true)
            {
                if (_playerIsNear && _isAttacking)
                {
                    _playerHealth.Decrement();
                    yield return new WaitForSeconds(attackRateInSeconds);
                }
                else yield return null;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.GetComponent<PlayerHealth>()) return;
            _playerIsNear = true;
        }
        private void OnTriggerExit(Collider other)
        {
            if (!other.GetComponent<PlayerHealth>()) return;
            _playerIsNear = false;
        }
    }
}