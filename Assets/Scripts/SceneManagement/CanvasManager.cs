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
        [SerializeField] private Transform emptyStarImagePrefab;
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
        
        public void OnClickPause()
        {
            Time.timeScale = 0;
            ShowCurrent(pauseCanvas);
        }

        public void OnClickResume()
        {
            Time.timeScale = 1;
            ShowCurrent(resumeCanvas);
        }

        private void Start()
        {
            Time.timeScale = 1;
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
            Time.timeScale = 0;
            DestroyEnemies();
            ShowCurrent(gameOverCanvas);
        }

        private void OnWin()
        {
            Time.timeScale = 0;
            ShowCurrent(winCanvas);
            DestroyEnemies();
            var starsCount = _statsManager.CalculateScore(maxStars);
            for (var i = 0; i < maxStars; i++)
            {
                var prefab = i < starsCount ? starImagePrefab : emptyStarImagePrefab;
                Instantiate(prefab, starsContentPane);
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