using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private bool drawGizmos = true;
        [SerializeField] private Button bombBtn;
        [SerializeField] private Bomb.Bomb bombPrefab;
        [Header("Attack settings")]
        [SerializeField] private float reloadTimeInSeconds = 3f;
        [SerializeField] private float dirtyTimeInSeconds = 3f;
        [SerializeField] private float explosionRadius = 3f;
        [SerializeField] private float explosionDelayInSeconds = 3f;
        [Header("Placement settings")] 
        [SerializeField] private float overlapCircleRadius = 1.5f;

        
        private bool _ready = true;

        public void OnClickPlaceButton()
        {
           if (!_ready) return;
           var spawnPos = GetBombSpawnPosition();
           var bomb = Instantiate(bombPrefab, spawnPos, Quaternion.identity);
           bomb.SetValues(explosionRadius, explosionDelayInSeconds, dirtyTimeInSeconds);
           StartCoroutine(ReloadCoroutine());
        }

        private Vector2 GetBombSpawnPosition()
        {
            float minDistance = float.PositiveInfinity;
            var spawnPosition = transform.position;
            foreach (var coll in Physics2D.OverlapCircleAll(transform.position, overlapCircleRadius))
            {
                if(!coll.CompareTag("Dot")) continue;
                var distance = (transform.position - coll.transform.position).sqrMagnitude;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    spawnPosition = coll.transform.position;
                }
            }
            return spawnPosition;
        }
        
        private IEnumerator ReloadCoroutine()
        {
            _ready = false;
            bombBtn.enabled = false;
            float currentTime = 0;
            while (currentTime < reloadTimeInSeconds)
            {
                currentTime += Time.deltaTime;
                bombBtn.image.fillAmount = currentTime / reloadTimeInSeconds;
                yield return null;
            }

            bombBtn.enabled = true;
            _ready = true;
        }

        private void OnDrawGizmosSelected()
        {
            if (!drawGizmos) return;
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, overlapCircleRadius);
        }
    }
}