using System.Collections;
using UnityEngine;
using TMPro;
using DotRun.UI;

namespace DotRun.Core
{
    public class ScoreManager : MonoBehaviour
    {
        // This default configuration makes 6 scales of difficulty and goes from 2.5 second to 1 second of maxDotTime
        [Tooltip("Maximum time the player will have to tap the next dot")]
        [SerializeField] private float maxDotTime = 2.5f;
        [SerializeField] private float timer = 0;
        [Tooltip("Amount of time to subtract in the maximum time to tap a dot and increase difficulty")]
        [SerializeField] private float decreaseMaxDotTime = 0.25f;
        [Tooltip("Number of dots taped to reach the maximum difficulty")]
        [SerializeField] private int maxDotsDifficulty = 150;
        [Tooltip("Number of dots to step up the maximum difficulty")]
        [SerializeField] private int dotsDifficultyStep = 25;
        [Tooltip("Number of dots that the player has tapped correctly")]
        [SerializeField] private int totalDotsScored = 0;

        [SerializeField] private int score = 0;
        [SerializeField] private int displayScore = 0;

        [SerializeField] private Timer timerUI = null;
        [SerializeField] private TextMeshProUGUI scoreUI;

        public bool timeRunning = false;

        private void Awake()
        {
            if (!timerUI)
                timerUI = FindObjectOfType<Timer>();
        }

        private void Update()
        {
            if (timeRunning)
            {
                // Difficulty increases every time we meet the condition till we reach the maximum
                if (totalDotsScored <= maxDotsDifficulty && totalDotsScored > 0 && totalDotsScored % dotsDifficultyStep == 0)
                    maxDotTime -= decreaseMaxDotTime;

                timer += Time.deltaTime;
                if (timer >= maxDotTime)
                {
                    GameManager.Instance.Hurt();
                    timerUI.ResetImageFill();
                    timer = 0;
                }

                timerUI.FillImage(maxDotTime, 0);
            }
        }

        public void ScorePoints(int points, float timeGain)
        {
            score += points;
            timer -= timeGain;
            if (timer < 0)
                timer = 0;
            timerUI.FillImage(maxDotTime, timeGain);
            // Manage the text score update incrementally
            StartCoroutine(ScoreUpdater());
        }

        private IEnumerator ScoreUpdater()
        {
            while (displayScore < score)
            {
                // Increment the display score by 1
                displayScore++;
                // Write it to the UI
                scoreUI.text = displayScore.ToString();
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}
