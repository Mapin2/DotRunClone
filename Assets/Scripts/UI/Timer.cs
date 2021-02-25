using UnityEngine;
using UnityEngine.UI;

namespace DotRun.UI
{
    public class Timer : MonoBehaviour
    {
        public Image timerFillImage = null;

        public void FillImage(float fillTime, float timeGain)
        {
            float time = Time.deltaTime - timeGain;
            timerFillImage.fillAmount += 1 / fillTime * time;
        }

        public void ResetImageFill()
        {
            timerFillImage.fillAmount = 0;
        }
    }
}

