using UnityEngine;
using UnityEngine.UI;
using DotRun.GamePlay;
using DotRun.Core;

namespace DotRun
{
    public class CurrentMaterial : MonoBehaviour
    {
        [SerializeField] private MapGenerator mapGenerator = null;
        private SpriteRenderer ownRenderer = null;
        private Image image = null;

        private void Awake()
        {
            if (!mapGenerator)
                mapGenerator = FindObjectOfType<MapGenerator>();

            // Current material event subscription
            mapGenerator.OnCurrentMaterialChange += ApplyCurrentMaterial;
        }

        private void Start()
        {
            TryGetComponent(out ownRenderer);
            TryGetComponent(out image);
            ApplyCurrentMaterial();
        }

        public void ApplyCurrentMaterial()
        {
            if (ownRenderer)
                ownRenderer.material = GameManager.Instance.currentMaterial;

            if (image)
                image.material = GameManager.Instance.currentMaterial;
        }
    }
}
