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
        private float waitSecondsAd = 0.005f;

        public void StartChangeLevel(int sceneId) {
            // Level change with coroutine duration waitSeconds
            StartCoroutine(ChangeLevel(sceneId));
        }

        public void StartChangeLevelAd(int sceneId)
        {
            // Level change with coroutine duration waitSeconds and intertititalAd
            AdManager.Instance.Interstitial();
            StartCoroutine(ChangeLevelAd(sceneId));
        }

        public void StartChangeLevelFrame(int sceneId) {
            // Level change with coroutine duration 1 frame
            StartCoroutine(ChangeLevelOneFrame(sceneId));
        }

        public void ChangeLevelNoDelay(int sceneId)
        {
            // Level change with no coroutine
            levelToLoad = sceneId;
            OnFadeComplete();
        }

        private IEnumerator ChangeLevel(int sceneId)
        {
            levelToLoad = sceneId;
            yield return new WaitForSeconds(waitSeconds);
            OnFadeComplete();
        }

        private IEnumerator ChangeLevelAd(int sceneId)
        {
            levelToLoad = sceneId;
            yield return new WaitForSeconds(waitSecondsAd);
            OnFadeComplete();
        }

        private IEnumerator ChangeLevelOneFrame(int sceneId)
        {
            levelToLoad = sceneId;
            yield return null;
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
