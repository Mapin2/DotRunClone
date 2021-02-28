using UnityEngine;
using UnityEngine.UI;
using DotRun.Utils;
using DotRun.Core;

namespace DotRun.Menu
{
    public class MainMenu : MonoBehaviour
    {
        public Button intertitialButton = null;

        public void Play()
        {
            SceneLoaderManager.Instance.StartChangeLevel(Constants.SCENE_INDEX_GAME);
        }

        public void Intertitial()
        {
            AdManager.Instance.Intertitial();
        }
    }
}

