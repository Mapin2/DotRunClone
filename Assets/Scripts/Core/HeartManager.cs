using UnityEngine;
using DotRun.UI;
using DotRun.Utils;
using System;

namespace DotRun.Core
{
    public class HeartManager : Singleton<HeartManager>
    {
        public Heart[] lifes = null;
        [HideInInspector] public bool isAlive = true;
        [HideInInspector] public bool hasAllLives = true;

        public void Hurt()
        {
            foreach (Heart heart in lifes)
            {
                if (heart.animatedIconHandler.isClicked)
                {
                    hasAllLives = false;
                    heart.animatedIconHandler.ClickEvent();
                    break;
                }
            }
            CheckAlive();
        }

        public void Heal()
        {
            // For healing the right heart
            Heart heartToHeal = null;
            foreach (Heart heart in lifes)
            {
                if (!heart.animatedIconHandler.isClicked)
                {
                    heartToHeal = heart;
                }
            }

            if (heartToHeal)
                heartToHeal.animatedIconHandler.ClickEvent();

            CheckHasAllLifes();
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

        private void CheckHasAllLifes()
        {
            bool anyHeartIsHurted = false;
            foreach (Heart heart in lifes)
            {
                if (!heart.animatedIconHandler.isClicked)
                    anyHeartIsHurted = true;
            }

            if (!anyHeartIsHurted)
                hasAllLives = true;
        }
    }
}

