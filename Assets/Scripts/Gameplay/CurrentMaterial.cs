using UnityEngine;
using DotRun.GamePlay;
using DotRun.Core;

namespace DotRun
{
    public class CurrentMaterial : MonoBehaviour
    {
        [SerializeField] private MapGenerator mapGenerator = null;
        [SerializeField] private SpriteRenderer ownRenderer = null;

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
            ApplyCurrentMaterial();
        }

        public void ApplyCurrentMaterial()
        {
            if (ownRenderer)
                ownRenderer.material = GameManager.Instance.currentMaterial;
        }
    }
}
