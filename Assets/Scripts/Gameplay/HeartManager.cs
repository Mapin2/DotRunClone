using UnityEngine;
using DotRun.UI;
using System;

namespace DotRun
{
    public class HeartManager : MonoBehaviour
    {
        public Heart[] lifes = null;
        public bool isAlive = true;

        public void Hurt()
        {
            foreach (Heart heart in lifes)
            {
                if (heart.animatedIconHandler.isClicked)
                {
                    heart.animatedIconHandler.ClickEvent();
                    break;
                }
            }
            CheckAlive();
        }

        public void Heal()
        {
            foreach (Heart heart in lifes)
            {
                if (!heart.animatedIconHandler.isClicked)
                {
                    heart.animatedIconHandler.ClickEvent();
                    break;
                }
            }
        }
        private void CheckAlive()
        {
            bool anyHeartsAlive = false;
            foreach (Heart heart in lifes)
            {
                if (heart.animatedIconHandler.isClicked)
                    anyHeartsAlive = true;
            }

            if (!anyHeartsAlive)
                isAlive = false;
        }
    }
}

