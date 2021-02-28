using UnityEngine;
using UnityEngine.UI;
using DotRun.Core;
using DotRun.GamePlay;
using DotRun.Utils;

namespace DotRun.Menu
{
    public class GameOverMenu : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverPanelUI = null;
        public Button rewardButton = null;

        public void ActivateMenu()
        {
            AdManager.Instance.bannerAd.Show();
            gameOverPanelUI.SetActive(true);
        }

        public void DeactivateMenu()
        {
            gameOverPanelUI.SetActive(false);
        }

        public void Exit()
        {
            // This is executed when the plater decides to end the match (and not run a reward add to keep playing)
            int maxScore = GameManager.Instance.maxScore;
            Material currentMaterial = Dot.latestTouchedDotMaterial;
            PlayerPrefs.SetString(Constants.PLAYERPREF_CURRENT_MATERIAL, currentMaterial.name);
            if (ScoreManager.Instance.score > maxScore)
            {
                maxScore = ScoreManager.Instance.score;
                PlayerPrefs.SetInt(Constants.PLAYERPREF_MAX_CURRENT_SCORE, maxScore);
            }

            SceneLoaderManager.Instance.StartChangeLevelFrame(Constants.SCENE_INDEX_MAIN_MENU);
        }

        public void RunReward()
        {
            AdManager.Instance.Reward();
        }
    }
}
