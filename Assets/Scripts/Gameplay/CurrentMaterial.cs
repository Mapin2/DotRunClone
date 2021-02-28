using UnityEngine;
using UnityEngine.UI;
using DotRun.Core;
using TMPro;

namespace DotRun.GamePlay
{
    public class CurrentMaterial : MonoBehaviour
    {
        [SerializeField] private MapGenerator mapGenerator = null;

        // Different types of components to change its color/material
        private SpriteRenderer spriteRenderer = null;
        private Image image = null;
        private TextMeshProUGUI text = null;

        private void Awake()
        {
            if (!mapGenerator)
                mapGenerator = FindObjectOfType<MapGenerator>();
            
            // Current material event subscription
            if (mapGenerator)
                mapGenerator.OnCurrentMaterialChange += ApplyCurrentMaterial;
        }

        private void Start()
        {
            // Try get component to which this script is attached
            TryGetComponent(out spriteRenderer);
            TryGetComponent(out image);
            TryGetComponent(out text);

            ApplyCurrentMaterial();
        }

        public void ApplyCurrentMaterial()
        {
            if (spriteRenderer)
                spriteRenderer.material = GameManager.Instance.currentMaterial;

            if (image)
                image.material = GameManager.Instance.currentMaterial;

            if (text)
                text.color = GameManager.Instance.currentMaterial.color;
        }
    }
}
