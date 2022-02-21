using System.Collections.Generic;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SceneManagement
{
    public class ObjectPlacer : MonoBehaviour
    {
        [SerializeField] private bool drawGizmos = true;
        [SerializeField] private GameObject prefab;
        [SerializeField] private int minObjectCount;
        [SerializeField] private int maxObjectCount;
        [SerializeField] private float minSpawnDistance;
        [SerializeField] private float maxCheckAttempts = 30;
        private int _startObjectCount;
        private float _sqrMinSpawnDistance;
        private List<Vector2> dots;

        private void Start()
        {
            dots = new List<Vector2>();
            _sqrMinSpawnDistance = minSpawnDistance * minSpawnDistance;
            _startObjectCount = Random.Range(minObjectCount, maxObjectCount);
            if (prefab.CompareTag("Carrot"))
            {
                FindObjectOfType<StatsManager>().SetStartCarrotCount(_startObjectCount);
            }
            FindDots();
            SpawnObjects();
        }

        private void FindDots()
        {
            Vector2 playerPosition = FindObjectOfType<PlayerMovement>().transform.position;
            foreach (var dot in GameObject.FindGameObjectsWithTag("Dot"))
            {
                Vector2 dotPosition = dot.transform.position;
                var sqrDistance = (dotPosition - playerPosition).sqrMagnitude;
                if (sqrDistance < _sqrMinSpawnDistance) continue;
                dots.Add(dotPosition);
            }
        }

        private void SpawnObjects()
        {
            var spawnedObjectsPositions = new List<Vector2>();
            var attempts = 0;

            for (var i = 0; i < _startObjectCount; i++)
            {
                var spawnPosition = dots[Random.Range(0, dots.Count)];

                if (!CheckDistanceToSpawnedObjects(spawnPosition, spawnedObjectsPositions))
                {
                    attempts++;
                    if (attempts < maxCheckAttempts)
                    {
                        i--;
                        continue;
                    }

                    attempts = 0;
                }

                dots.Remove(spawnPosition);
                spawnedObjectsPositions.Add(spawnPosition);
                Instantiate(prefab, spawnPosition, Quaternion.identity);
            }
        }

        private bool CheckDistanceToSpawnedObjects(Vector2 spawnPosition, List<Vector2> objectsPositions)
        {
            foreach (var objectPosition in objectsPositions)
            {
                var sqrDistance = (spawnPosition - objectPosition).sqrMagnitude;
                if (sqrDistance < _sqrMinSpawnDistance) return false;
            }
            return true;
        }

        private void OnDrawGizmosSelected()
        {
            if (!drawGizmos) return;
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, minSpawnDistance);
        }
    }
}