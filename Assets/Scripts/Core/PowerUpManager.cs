using UnityEngine;
using DotRun.Utils;
using DotRun.GamePlay;

namespace DotRun.Core
{
    public class PowerUpManager : Singleton<PowerUpManager>
    {
        public GameObject powerUpIndicator = null;
        public GameObject[] powerUpIndicatorTypes = null;

        private float minSecsToSpawnPowerUp = 5;
        private float maxSecsToSpawnPowerUp = 10;
        private float timer = 0;
        private float nextPowerUp = 0;
        private float powerUpDuration = 0;

        public bool canSpawnPowerUp = false;
        public bool powerUpSpawned = false;
        public bool hasPowerUp = false;

        private void Start()
        {
            ReloadTimer();
        }

        private void Update()
        {

            if (GameManager.Instance.gameIsRunning && !canSpawnPowerUp && !powerUpSpawned && !hasPowerUp)
            {
                SpawnPowerUpTimer();
            }

            if (GameManager.Instance.gameIsRunning && powerUpDuration > 0)
            {
                PowerUpTimer();
            }
        }

        private void SpawnPowerUpTimer()
        {
            timer += Time.deltaTime;
            if (timer >= nextPowerUp)
            {
                canSpawnPowerUp = true;
                ReloadTimer();
            }
        }
        private void PowerUpTimer()
        {
            powerUpDuration -= Time.deltaTime;
            if (powerUpDuration <= 0)
            {
                powerUpDuration = 0;
                ScoreManager.Instance.scoreMultiplier = 1;
                DeactivateIndicator();
            }
        }

        private void ReloadTimer()
        {
            nextPowerUp = Random.Range(minSecsToSpawnPowerUp, maxSecsToSpawnPowerUp);
            timer = 0;
        }

        public void ActivatePowerUp(GameObject dotObject)
        {
            // Change interactable type
            Dot dot = dotObject.GetComponentInChildren<Dot>();
            dot.type = InteractableType.PowerUp;
            
            // Pick a random power up from the list and activate it
            PowerUp[] powerUps = dot.powerUps;
            PowerUp powerUp = powerUps[Random.Range(0, powerUps.Length)];
            powerUp.gameObject.SetActive(true);

            powerUpSpawned = true;
            canSpawnPowerUp = false;
        }

        public void ActivateIndicator(PowerUpType type)
        {
            powerUpSpawned = false;
            hasPowerUp = true;
            powerUpIndicator.SetActive(true);

            // Find the correct indicator icon to show the current powerUP
            foreach (GameObject powerUpIndicatorType in powerUpIndicatorTypes)
            {
                if (type == PowerUpType.MultiplyX2 && powerUpIndicatorType.name.Equals(Constants.OBJECT_NAME_X2))
                    powerUpIndicatorType.SetActive(true);

                if (type == PowerUpType.MultiplyX3 && powerUpIndicatorType.name.Equals(Constants.OBJECT_NAME_X3))
                    powerUpIndicatorType.SetActive(true);
            }
        }

        public void DeactivateIndicator()
        {
            hasPowerUp = false;
            powerUpIndicator.SetActive(false);

            foreach (GameObject powerUpIndicatorType in powerUpIndicatorTypes)
                powerUpIndicatorType.SetActive(false);
        }

        public void StartMultiply(float duration)
        {
            // PowerUp timer
            powerUpDuration = duration;
        }
    }
}
