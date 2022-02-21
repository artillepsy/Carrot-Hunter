using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ReloadButton : MonoBehaviour
    {
        [SerializeField] private Button bombBtn;
        private float _reloadTime = 0;
        private float _timeSinceClick = 0;
        private bool _reloading = false;
        private void Start()
        {
            FindObjectOfType<PlayerAttack>().OnReload.AddListener(StartReload);
        }
        private void Update()
        {
            if (!_reloading) return;
            FillImage();
        }

        private void StartReload(float reloadTime)
        {
            _reloading = true;
            _timeSinceClick = 0f;
            _reloadTime = reloadTime;
            bombBtn.enabled = false;
        }
        private void FillImage()
        {
            if (_timeSinceClick > _reloadTime)
            {
                bombBtn.enabled = true;
                _reloading = false;
            }
            _timeSinceClick += Time.deltaTime;
            bombBtn.image.fillAmount = _timeSinceClick / _reloadTime;
        }

    }
}