using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private bool drawGizmos = true;
        [SerializeField] private Button bombBtn;
        [SerializeField] private Bomb bombPrefab;
        [SerializeField] private float reloadTimeInSeconds = 3f;
        [SerializeField] private float dirtyTimeInSeconds = 3f;
        [SerializeField] private float explosionRadius = 3f;
        [SerializeField] private float explosionDelayInSeconds = 3f;
        private bool _ready = true;

        public void OnClickPlaceButton()
        {
           if (!_ready) return;
           var bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
           bomb.SetValues(explosionRadius, explosionDelayInSeconds, dirtyTimeInSeconds);
           StartCoroutine(ReloadCoroutine());
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
        }
    }
}