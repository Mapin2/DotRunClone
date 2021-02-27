using UnityEngine;
using Michsky.UI.ModernUIPack;

namespace DotRun.UI
{
    public class Heart : MonoBehaviour
    {
        [HideInInspector] public AnimatedIconHandler animatedIconHandler = null;

        void Start()
        {
            if (!animatedIconHandler)
                animatedIconHandler = GetComponent<AnimatedIconHandler>();
        }
    }
}

