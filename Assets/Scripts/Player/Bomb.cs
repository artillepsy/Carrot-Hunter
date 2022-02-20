using System.Collections;
using System.Collections.Generic;
using Enemy;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class Bomb : MonoBehaviour
    {
        public static readonly UnityEvent OnCarrotExplose = new UnityEvent();
        [SerializeField] private Transform wick;
        [SerializeField] private ParticleSystem wickSparksParticleSystem;
        [SerializeField] private ParticleSystem explosionPartileSystem;
        [SerializeField] private float rotationMaxAngle = 3f;
        [SerializeField] private float rotationSpeed = 5f;

        [Header("Overlap Settings")]
        [Range(0, 45)]
        [SerializeField] private float maxDeviationAngle = 10f;

        
        private float _rotationTime = 0;
        
        public void SetValues(float explosionRadius, float explosionDelay, float dirtyTime)
        {
            StartCoroutine(ExploseCoroutine(explosionRadius, explosionDelay, dirtyTime));
        }

        private void Update()
        {
            _rotationTime += Time.deltaTime;
            transform.rotation = quaternion.Euler(0,0, Mathf.Sin(_rotationTime * rotationSpeed) * rotationMaxAngle); 
        }

        private IEnumerator ExploseCoroutine(float explosionRadius, float explosionDelay, float dirtyTime)
        {
            Instantiate(wickSparksParticleSystem, wick.position, Quaternion.identity, wick);
            yield return new WaitForSeconds(explosionDelay);
           
            var colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
            var dots = new List<Vector2>();
            foreach (var coll in colliders)
            {
                if(IsOverlapping(coll.transform.position)) continue;
                if (!coll.isTrigger && coll.CompareTag("Enemy"))
                {
                    coll.GetComponent<EnemyStateController>().MakeDirty(dirtyTime);
                    continue;
                }
                if (!coll.isTrigger && coll.CompareTag("Player"))
                {
                    coll.GetComponent<PlayerHealth>().Decrement();
                    continue;
                }
                if (coll.CompareTag("Carrot"))
                {
                    Destroy(coll.gameObject);
                    OnCarrotExplose?.Invoke();
                    continue;
                }
                if (!coll.CompareTag("Dot")) continue;
                dots.Add(coll.transform.position);
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

        private bool IsOverlapping(Vector2 position)
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