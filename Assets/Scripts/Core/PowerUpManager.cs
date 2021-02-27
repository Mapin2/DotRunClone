using System.Collections;
using UnityEngine;
using DG.Tweening;
using DotRun.Utils;
using DotRun.GamePlay;

namespace DotRun.Core
{
    public class PowerUpManager : Singleton<PowerUpManager>
    {
        public GameObject powerUpIndicator = null;
        [SerializeField] private float powerUpIndicatorActivePos = 421;
        [SerializeField] private float powerUpIndicatorUnactivePos = 460;
        private float animDuration = 0.5f;
        public GameObject[] powerUpIndicatorTypes = null;

        [SerializeField] private float minSecsToSpawnPowerUp = 5;
        [SerializeField] private float maxSecsToSpawnPowerUp = 10;
        private float timer = 0;
        private float nextPowerUp = 0;

        public bool canSpawnPowerUp = false;
        public bool powerUpSpawned = false;
        public bool hasPowerUp = false;

        private void Start()
        {
            ReloadTimer();
        }

        private void Update()
        {
            if (!canSpawnPowerUp && !powerUpSpawned && !hasPowerUp)
            {
                timer += Time.deltaTime;
                if (timer >= nextPowerUp)
                {
                    canSpawnPowerUp = true;
                    ReloadTimer();
                }
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
            // TODO encontrar una manera de que no se rompa la posicion por culpa de que es un objeto del canvas
            // powerUpIndicator.transform.DOLocalMoveY(powerUpIndicatorActivePos, animDuration).SetEase(Ease.OutQuint);

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
            // TODO encontrar una manera de que no se rompa la posicion por culpa de que es un objeto del canvas
            // powerUpIndicator.transform.DOLocalMoveY(powerUpIndicatorUnactivePos, animDuration).SetEase(Ease.InQuint);

            foreach (GameObject powerUpIndicatorType in powerUpIndicatorTypes)
                powerUpIndicatorType.SetActive(false);
        }

        internal void StartMultiply(float duration)
        {
            StartCoroutine(Multiplier(duration));
        }

        public IEnumerator Multiplier(float duration)
        {
            yield return new WaitForSeconds(duration);
            ScoreManager.Instance.scoreMultiplier = 1;
            DeactivateIndicator();
        }
    }
}
