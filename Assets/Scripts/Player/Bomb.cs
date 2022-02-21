using System.Collections.Generic;
using Core;
using Unity.Mathematics;
using UnityEngine;

namespace Player
{
    public class Bomb : MonoBehaviour
    {
        [SerializeField] private Transform wick;
        [SerializeField] private ParticleSystem wickSparksParticleSystem;
        [SerializeField] private ParticleSystem explosionPartileSystem;
        [SerializeField] private float rotationMaxAngle = 1f;
        [SerializeField] private float rotationSpeed = 5f;

        [Header("Overlap Settings")]
        [Range(0, 45)]
        [SerializeField] private float maxDeviationAngle = 15f;
        
        private float _rotationTime = 0;
        private float _timeSinceSpawned = 0f;
        
        private bool _canExplode = false;
        private float _explosionDelay;
        private float _explosionRadius;
       
        public void SetValues(float explosionRadius, float explosionDelay)
        {
            _explosionDelay = explosionDelay;
            _explosionRadius = explosionRadius;
            _canExplode = true;
            Instantiate(wickSparksParticleSystem, wick.position, Quaternion.identity, wick);
        }

        private void Update()
        {
            if(IsTimeToExplode()) Explode();
            RotateBomb();
        }
        
        private bool IsTimeToExplode()
        {
            if (!_canExplode) return false;
            if (_timeSinceSpawned >= _explosionDelay)
            {
                _timeSinceSpawned = 0f;
                return true;
            }
            _timeSinceSpawned+=Time.deltaTime;
            return false;
        }
        
        private void RotateBomb()
        {
            _rotationTime += Time.deltaTime;
            transform.rotation = quaternion.Euler(0,0, Mathf.Sin(
                _rotationTime * rotationSpeed) * rotationMaxAngle);
        }
        
        private void Explode()
        {
            var dots = new List<Vector2>();
            var colliders = Physics2D.OverlapCircleAll(transform.position, _explosionRadius);
            foreach (var coll in colliders)
            {
                if(IsObstacleBetweenPoints(coll.transform.position)) continue;
                if(coll.CompareTag("Dot")) dots.Add(coll.transform.position);
                else coll.GetComponent<IAffectOnDamage>()?.OnDamaged();
            }
            SpawnParticles(dots);
            Destroy(gameObject);
        }
        
        private void SpawnParticles(List<Vector2> dots)
        {
            foreach (var dot in dots)
            {
                Instantiate(explosionPartileSystem, dot, Quaternion.identity);
            }
        }

        private bool IsObstacleBetweenPoints(Vector2 position)
        {
            var currentVector = position - (Vector2) transform.position;
            var deviationAngle = Vector2.Angle(Vector2.up, currentVector);
            if (deviationAngle > maxDeviationAngle && deviationAngle < 90 - maxDeviationAngle ||
                deviationAngle > 90 + maxDeviationAngle && deviationAngle < 180 - maxDeviationAngle)
            {
                return true;
            }
            foreach (var hit in Physics2D.LinecastAll(position, transform.position))
            {
                if (hit.collider.CompareTag("Obstacle")) return true;
            }
            return false;
        }
    }
}