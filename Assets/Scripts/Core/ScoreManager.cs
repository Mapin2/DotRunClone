using System.Collections;
using UnityEngine;
using TMPro;
using DotRun.Utils;
using DotRun.UI;

namespace DotRun.Core
{
    public class ScoreManager : Singleton<ScoreManager>
    {
        // This default configuration makes 6 scales of difficulty and goes from 2.5 second to 1 second of maxDotTime
        private float maxDotTime = 2.5f; // Maximum time the player will have to tap the next dot
        private float decreaseMaxDotTime = 0.25f; // Amount of time to subtract in the maximum time to tap a dot and increase difficulty
        private int maxDotsDifficulty = 150; // Number of dots taped to reach the maximum difficulty
        private int dotsDifficultyStep = 25; // Number of dots to step up the maximum difficulty
        private int totalDotsScored = 0; // Number of dots that the player has tapped correctly

        private float timer = 0;
        [HideInInspector] public int score = 0;
        private int displayScore = 0;
        [HideInInspector] public int scoreMultiplier = 1;

        [Header("References")]
        [SerializeField] private Timer timerUI = null;
        [SerializeField] private TextMeshProUGUI scoreUI = null;
        [SerializeField] private TextMeshProUGUI scorePauseUI = null;

        public override void Awake()
        {
            base.Awake();

            if (!timerUI)
                timerUI = FindObjectOfType<Timer>();
        }

        private void Update()
        {
            if (GameManager.Instance.gameIsRunning)
            {
                // Difficulty increases every time we meet the condition till we reach the maximum
                if (totalDotsScored <= maxDotsDifficulty && totalDotsScored > 0 && totalDotsScored % dotsDifficultyStep == 0)
                    maxDotTime -= decreaseMaxDotTime;
                
                Timer();

                timerUI.FillImage(maxDotTime, 0);
            }
        }

        private void Timer()
        {
            timer += Time.deltaTime;
            if (timer >= maxDotTime)
            {
                AudioManager.Instance.PlaySound(SoundType.Hurt);
                GameManager.Instance.Hurt();
                timerUI.ResetImageFill();
                timer = 0;
            }
        }

        public void ScorePoints(int points, float timeGain)
        {
            score += points * scoreMultiplier;
            scorePauseUI.text = score.ToString();

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

        public void ReloadTimerAndUI()
        {
            timer = 0;
            timerUI.ResetImageFill();
        }
    }
}
