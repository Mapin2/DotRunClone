using System;
using UnityEngine;
using DotRun.Core;
using DotRun.Utils;

namespace DotRun.GamePlay
{
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField] private Material[] dotMaterials = null;
        [SerializeField] private int[] possibleXPositions = new int[] { -2, 0, 2 };
        [SerializeField] private GameObject dotPrefab = null;
        [SerializeField] private Material currentMaterial = null;

        [SerializeField] private int maxYDotPosition = 2;
        [SerializeField] private int minYDotPosition = -2;
        [SerializeField] private int maxXDotPosition = 2;
        [SerializeField] private int minXDotPosition = -2;

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
            for (int yPos = maxYDotPosition; yPos >= minYDotPosition; yPos -= 2)
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
                // Generate new row of dots for changing the current material
                InstantiateRow(maxYDotPosition, true);
                CalculateChangeCurrentMaterial();
            }

        }

        private void InstantiateRow(int yPos, bool changeCurrentMaterial)
        {
            if (!changeCurrentMaterial)
            {
                // Calculate which X position will have the mandatory currentMaterial dot
                int currentMaterialDotX = possibleXPositions[UnityEngine.Random.Range(0, possibleXPositions.Length)];
                for (int xPos = minXDotPosition; xPos <= maxXDotPosition; xPos += 2)
                {
                    Material material = xPos == currentMaterialDotX ? currentMaterial : GetRandomMaterial();
                    InstantiateDot(new Vector2(xPos, yPos), material, changeCurrentMaterial);
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
               
                for (int xPos = minXDotPosition; xPos <= maxXDotPosition; xPos += 2)
                {
                    InstantiateDot(new Vector2(xPos, yPos), material, changeCurrentMaterial);
                }
            }

        }

        private void InstantiateDot(Vector2 position, Material material, bool changeCurrentMaterial)
        {
            GameObject instantiatedDot = Instantiate(dotPrefab, position, Quaternion.identity);
            instantiatedDot.GetComponent<SpriteRenderer>().material = material;
            instantiatedDot.layer = !changeCurrentMaterial ? Constants.LAYER_NOT_INTERACTABLE_DOT : Constants.LAYER_CHANGE_CURRENT_MATERIAL;
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
