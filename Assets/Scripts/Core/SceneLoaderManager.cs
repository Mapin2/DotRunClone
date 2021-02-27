using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DotRun.Utils;

namespace DotRun.Core
{
    public class SceneLoaderManager : Singleton<SceneLoaderManager>
    {
        [SerializeField] Animator fadeAnimator = null;

        private int levelToLoad = 0;

        private float waitSeconds = 0.3f;

        public IEnumerator ChangeLevel(int sceneId)
        {
            levelToLoad = sceneId;
            yield return new WaitForSeconds(waitSeconds);
            OnFadeComplete();
        }

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
