using UnityEngine;
using UnityEngine.SceneManagement;
using DotRun.Utils;
using DotRun.Core;


namespace DotRun.Menu
{
    public class MainMenu : MonoBehaviour
    {
        public void Play()
        {
            StartCoroutine(SceneLoaderManager.Instance.ChangeLevel(Constants.SCENE_INDEX_GAME));
        }
    }
}

