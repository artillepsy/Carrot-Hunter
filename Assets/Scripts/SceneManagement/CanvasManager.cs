using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


namespace SceneManagement
{
    public class CanvasManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> canvases;
        [Space]
        [SerializeField] private GameObject pauseCanvas;
        [Space]
        [SerializeField] private GameObject resumeCanvas;
        [Space]
        [SerializeField] private GameObject gameOverCanvas;
        [Space]
        [SerializeField] private GameObject winCanvas;
        [SerializeField] private Transform starsContentPane;
        [SerializeField] private Transform starImagePrefab;
        [SerializeField] private int maxStars = 4;
        private StatsManager _statsManager;
        
        public void OnClickRestart()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }

        public void OnClickExit()
        {
            Application.Quit();
        }
        
        public void OnClickPause() => ShowCurrent(pauseCanvas);
        public void OnClickResume() => ShowCurrent(resumeCanvas);
        private void Start()
        {
            _statsManager = FindObjectOfType<StatsManager>();
            _statsManager.OnGameOver.AddListener(OnGameOver);
            _statsManager.OnWin.AddListener(OnWin);
            ShowCurrent(resumeCanvas);
        }

        private void ShowCurrent(GameObject currentCanvas)
        {
            foreach (var canvas in canvases)
            {
                if(canvas != currentCanvas) canvas.SetActive(false);
                else canvas.SetActive(true);
            }
        }

        private void OnGameOver()
        {
            DestroyEnemies();
            ShowCurrent(gameOverCanvas);
        }

        private void OnWin()
        {
            ShowCurrent(winCanvas);
            DestroyEnemies();
            var starsCount = _statsManager.CalculateScore(maxStars);
            for (int i = 0; i < starsCount; i++)
            {
                Instantiate(starImagePrefab, starsContentPane);
            }
        }

        private void DestroyEnemies()
        {
            foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                Destroy(enemy);
            }
        }
    }
}