using UnityEngine;
using UnityEngine.UI;

namespace DotRun.Menu
{
    public class IconAlpha : MonoBehaviour
    {
        [SerializeField] private Button rewardButton = null;
        [SerializeField] private Image childIcon = null;

        private void Update()
        {
            if (!rewardButton.interactable)
            {
                Color color = childIcon.color;
                color.a = .5f;
                childIcon.color = color;
            }
            else
            {
                Color color = childIcon.color;
                color.a = 1f;
                childIcon.color = color;
            }
        }
    }
}
