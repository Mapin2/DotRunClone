using System;
using UnityEngine;
using DotRun.Core;
using DotRun.Utils;

namespace DotRun.GamePlay
{
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField] private Material[] dotMaterials = null;
        [SerializeField] private float[] possibleXPositions = new float[] { -1.5f, 0, 1.5f };
        [SerializeField] private GameObject dotPrefab = null;
        [SerializeField] private GameObject trianglePrefab = null;
        [SerializeField] private Material currentMaterial = null;

        [SerializeField] private float maxYDotPosition = 2;
        [SerializeField] private float minYDotPosition = -2.5f;
        [SerializeField] private float maxXDotPosition = 1.5f;
        [SerializeField] private float minXDotPosition = -1.5f;
        [SerializeField] private float displacement = -1.5f;

        [SerializeField] private int numberOfDotsToChangeCurrentMaterial = 0;
        [SerializeField] private int minDotsToChangeCurrentMaterial = 5;
        [SerializeField] private int maxDotsToChangeCurrentMaterial = 30;

        public event Action OnDotTouched;
        public event Action OnCurrentMaterialChange;

        public void Awake()
        {
            // Retrieve current material reference to generate the map based on this color and use it for the UI
            currentMaterial = GameManager.Instance.currentMaterial;
        }

        public void GenerateMap()
        {
            for (float yPos = maxYDotPosition; yPos >= minYDotPosition; yPos -= displacement)
                InstantiateRow(yPos, false);

            CalculateChangeCurrentMaterial();
        }

        public void ScrollMap()
        {
            // Fire event OnDotTouched if theres any suscriber (I'm susribing all Dots Movement)
            OnDotTouched?.Invoke();

            numberOfDotsToChangeCurrentMaterial--;
            if (numberOfDotsToChangeCurrentMaterial >= 0)
            {
                // Generate new row of dots
                InstantiateRow(maxYDotPosition, false);
            }
            else
            {
                // Generate new row for changing the current material
                InstantiateRow(maxYDotPosition, true);
                CalculateChangeCurrentMaterial();
            }

        }

        private void InstantiateRow(float yPos, bool changeCurrentMaterial)
        {
            if (!changeCurrentMaterial)
            {
                // Calculate which X position will have the mandatory currentMaterial dot
                float currentMaterialDotX = possibleXPositions[UnityEngine.Random.Range(0, possibleXPositions.Length)];
                for (float xPos = minXDotPosition; xPos <= maxXDotPosition; xPos += displacement)
                {
                    bool isCurrent = xPos == currentMaterialDotX ? true : false;
                    Material material = isCurrent ? currentMaterial : GetRandomMaterial();
                    GameObject instantiatedDot = InstantiateDot(new Vector2(xPos, yPos), material, changeCurrentMaterial);
                    if (isCurrent && PowerUpManager.Instance.canSpawnPowerUp && !PowerUpManager.Instance.powerUpSpawned)
                        PowerUpManager.Instance.ActivatePowerUp(instantiatedDot);
                }
            }
            else
            {
                Material material;
                do
                {
                    material = GetRandomMaterial();
                } while (material == currentMaterial);

                currentMaterial = material;
                GameManager.Instance.currentMaterial = currentMaterial;
               
                for (float xPos = minXDotPosition; xPos <= maxXDotPosition; xPos += displacement)
                {
                    InstantiateDot(new Vector2(xPos, yPos), material, changeCurrentMaterial);
                }
            }

        }

        private GameObject InstantiateDot(Vector2 position, Material material, bool changeCurrentMaterial)
        {
            GameObject instantiatedDot = Instantiate(!changeCurrentMaterial ? dotPrefab : trianglePrefab, position, Quaternion.identity);
            instantiatedDot.GetComponent<SpriteRenderer>().material = material;
            instantiatedDot.layer = Constants.LAYER_NOT_INTERACTABLE_DOT;
            return instantiatedDot;
        }

        private Material GetRandomMaterial()
        {
            return dotMaterials[UnityEngine.Random.Range(0, dotMaterials.Length)];
        }

        private void CalculateChangeCurrentMaterial()
        {
            numberOfDotsToChangeCurrentMaterial = UnityEngine.Random.Range(minDotsToChangeCurrentMaterial, maxDotsToChangeCurrentMaterial + 1);
        }

        public void ApplyCurrentColorChange()
        {
            OnCurrentMaterialChange?.Invoke();
        }
    }
}
