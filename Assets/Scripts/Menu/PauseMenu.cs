using UnityEngine;
using DotRun.Core;
using DotRun.Utils;

namespace DotRun.UI
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject pauseMenuPanel = null;

        public void Pause()
        {
            GameManager.Instance.gameIsRunning = false;
            pauseMenuPanel.SetActive(true);
            AdManager.Instance.bannerAd.Show();
        }

        public void Resume()
        {
            AdManager.Instance.bannerAd.Hide();
            if (GameManager.Instance.gameStarted)
                GameManager.Instance.gameIsRunning = true;

            pauseMenuPanel.SetActive(false);
        }

        public void Exit()
        {
            SceneLoaderManager.Instance.ChangeLevelNoDelay(Constants.SCENE_INDEX_MAIN_MENU);
        }
    }
}

