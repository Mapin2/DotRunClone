using UnityEngine;
using UnityEngine.SceneManagement;
using DotRun.Utils;
using DotRun.GamePlay;
using TMPro;

namespace DotRun.Core
{
    public class GameManager : Singleton<GameManager>
    {
        public Material currentMaterial = null;
        [SerializeField] private Material lastCurrentMaterial = null;
        public int maxScore = 0;
        [SerializeField] TextMeshProUGUI maxScoreUI = null;

        // Managers
        [SerializeField] private MapGenerator mapGenerator = null;
        [SerializeField] private HeartManager heartManager = null;
        [SerializeField] private ScoreManager scoreManager = null;

        public override void Awake()
        {
            // Singleton awake call
            base.Awake();

            // Load material saved on playerprefs if exist
            string currenMaterialName = PlayerPrefs.GetString(Constants.PLAYERPREF_CURRENT_MATERIAL, Constants.DEFAULT_CURRENT_MATERIAL);
            lastCurrentMaterial = Resources.Load(Constants.RESOURCES_MATERIALS_FOLDER + currenMaterialName, typeof(Material)) as Material;

            // Load max score saved on playerprefs if exist
            maxScore = PlayerPrefs.GetInt(Constants.PLAYERPREF_MAX_CURRENT_SCORE, 0);
            if (maxScoreUI)
                maxScoreUI.text = maxScore.ToString();

            // The current color will be used for some UI and generation of the map based on this material
            currentMaterial = lastCurrentMaterial;
            lastCurrentMaterial = null;
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.buildIndex == Constants.SCENE_INDEX_GAME)
            {
                // Since the Game Manager is loaded on the main menu, and the awake is called only once, we look for the managers when we enter in the game scene, and generate the map
                if (!mapGenerator)
                    mapGenerator = FindObjectOfType<MapGenerator>();

                if (!heartManager)
                    heartManager = FindObjectOfType<HeartManager>();

                if (!scoreManager)
                    scoreManager = FindObjectOfType<ScoreManager>();

                mapGenerator.GenerateMap();
            }
        }

        public void StartGame()
        {
            scoreManager.timeRunning = true;
        }

        public void Hurt()
        {
            heartManager.Hurt();
            if (!heartManager.isAlive)
                GameOver();
        }

        public void Heal()
        {
            heartManager.Heal();
        }

        public void ScorePoints(int points, float timeGain)
        {
            scoreManager.ScorePoints(points, timeGain);
        }

        private void GameOver()
        {
            scoreManager.timeRunning = false;
            currentMaterial = Dot.latestTouchedDotMaterial;
            PlayerPrefs.SetString(Constants.PLAYERPREF_CURRENT_MATERIAL, currentMaterial.name);
            if (scoreManager.score > maxScore)
            {
                maxScore = scoreManager.score;
                PlayerPrefs.SetInt(Constants.PLAYERPREF_MAX_CURRENT_SCORE, maxScore);
            }

            SceneLoaderManager.Instance.FadeToLevel(Constants.SCENE_INDEX_MAIN_MENU);
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
