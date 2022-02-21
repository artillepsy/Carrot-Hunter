using UnityEngine;
using UnityEngine.Events;
using Navigation = Core.Navigation;

namespace Player
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private bool drawGizmos = true;
        [SerializeField] private Bomb bombPrefab;
        [Header("Attack settings")]
        [SerializeField] private float reloadTimeInSeconds = 3f;
        [SerializeField] private float explosionRadius = 3f;
        [SerializeField] private float explosionDelayInSeconds = 3f;
        [Header("Placement settings")] 
        [SerializeField] private float overlapCircleRadius = 1.5f;
        public readonly UnityEvent<float> OnReload = new UnityEvent<float>();
        private bool _ready = true;
        private Navigation _navigation;

        private void Start()
        {
            _navigation = GetComponent<Navigation>();
        }

        public void OnClickPlaceButton()
        {
           if (!_ready) return;
           var spawnPos = _navigation.GetNearestDot(overlapCircleRadius);
           var bomb = Instantiate(bombPrefab, spawnPos, Quaternion.identity);
           bomb.SetValues(explosionRadius, explosionDelayInSeconds);
           OnReload?.Invoke(reloadTimeInSeconds);
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