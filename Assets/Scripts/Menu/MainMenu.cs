using UnityEngine;
using UnityEngine.SceneManagement;
using DotRun.Core;
using DotRun.Utils;


namespace DotRun.Menu
{
    public class MainMenu : MonoBehaviour
    {
        public void Play()
        {
            SceneLoaderManager.Instance.FadeToLevel(Constants.SCENE_INDEX_GAME);
        }
    }
}

