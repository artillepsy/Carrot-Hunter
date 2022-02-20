using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using Unity.Mathematics;
using UnityEngine;

namespace Player
{
    public class Bomb : MonoBehaviour
    {
        [SerializeField] private Transform wick;
        [SerializeField] private ParticleSystem wickSparksParticleSystem;
        [SerializeField] private ParticleSystem explosePartileSystem;
        [SerializeField] private float rotationMaxAngle = 3f;
        [SerializeField] private float rotationSpeed = 5f;
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
                if (coll.CompareTag("Enemy") && !IsStonesOverlap(coll.transform.position))
                {
                    coll.GetComponent<EnemyStateController>().MakeDirty(dirtyTime);
                    continue;
                }
                if (!coll.CompareTag("Dot")) continue;
                if(IsStonesOverlap(coll.transform.position)) continue;
                dots.Add(coll.transform.position);
            }
            SpawnParticles(dots);
            Destroy(gameObject);
        }
        
        private void SpawnParticles(List<Vector2> dots)
        {
            
        }

        private bool IsStonesOverlap(Vector2 position)
        {
            foreach (var hit in Physics2D.LinecastAll(position, transform.position))
            {
                if (hit.collider.CompareTag("Obstacle")) return true;
            }
            return false;
        }

        private void OnDestroy()
        {
            
        }
    }
}