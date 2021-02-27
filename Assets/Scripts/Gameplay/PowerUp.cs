using UnityEngine;
using DotRun.Utils;
using DotRun.Core;

namespace DotRun.GamePlay
{
    public class PowerUp : MonoBehaviour
    {
        [SerializeField] public PowerUpType type;
        [SerializeField] private int duration = 10;

        public void Trigger()
        {
            switch (type)
            {
                case PowerUpType.Heal:
                    GameManager.Instance.Heal();
                    break;
                case PowerUpType.MultiplyX2:
                    ScoreManager.Instance.scoreMultiplier = 2;
                    PowerUpManager.Instance.ActivateIndicator(type);
                    PowerUpManager.Instance.StartMultiply(duration);
                    break;
                case PowerUpType.MultiplyX3:
                    ScoreManager.Instance.scoreMultiplier = 3;
                    PowerUpManager.Instance.ActivateIndicator(type);
                    PowerUpManager.Instance.StartMultiply(duration);
                    break;
            }
        }
    }
}
