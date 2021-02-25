using UnityEngine;
using UnityEngine.SceneManagement;
using DotRun.Utils;

namespace DotRun.Core
{
    public class SceneLoaderManager : Singleton<SceneLoaderManager>
    {
        [SerializeField] Animator fadeAnimator = null;

        private int levelToLoad = 0;

        public void FadeToLevel(int sceneId)
        {
            levelToLoad = sceneId;
            fadeAnimator.SetTrigger(Constants.ANIM_FADE_OUT_ID);
        }

        public void OnFadeComplete()
        {
            SceneManager.LoadScene(levelToLoad);
        }
    }
}
