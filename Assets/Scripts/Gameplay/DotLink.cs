using UnityEngine;
using DotRun.Utils;

namespace DotRun.GamePlay
{
    public class DotLink : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera = null;

        [SerializeField] private Dot currentDot = null;
        [SerializeField] private Dot touchedDot = null;

        [SerializeField] private MapGenerator mapGenerator = null;

        [SerializeField] private float lineThickness = 0.5f;

        private void Awake()
        {
            if (!mainCamera)
                mainCamera = Camera.main;

            if (!mapGenerator)
                mapGenerator = FindObjectOfType<MapGenerator>();
        }

        private void Update()
        {
            ManageTouch();

            // First one, save and start game
            FirstDotTouched();

            // Next dots touched logic (Link, life, scroll map)
            LinkDots();
        }

        private void ManageTouch()
        {
            if (Input.touchCount > 0)
            {
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

                Transform hit = null;
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                    hit = Physics2D.Raycast(touchPosition, Vector2.zero).transform;

                if (hit && (hit.gameObject.layer == Constants.LAYER_INTERACTABLE_DOT || hit.gameObject.layer == Constants.LAYER_CHANGE_CURRENT_MATERIAL))
                    hit.TryGetComponent(out touchedDot);
            }
        }

        private void FirstDotTouched()
        {
            if (!currentDot && touchedDot)
            {
                currentDot = touchedDot;
                touchedDot = null;

                // Perform movement
                mapGenerator.ScrollMap();

                CreateLink(currentDot);

                // TODO: GameManager.Start para dar inicio al futuro contador 
            }
        }

        private void LinkDots()
        {
            if (touchedDot)
            {
                // If it is the same material create first link with line renderer
                if (touchedDot.dotMaterial == currentDot.dotMaterial || touchedDot.gameObject.layer == Constants.LAYER_CHANGE_CURRENT_MATERIAL)
                {
                    if (touchedDot.gameObject.layer == Constants.LAYER_CHANGE_CURRENT_MATERIAL)
                        mapGenerator.ApplyCurrentColorChange();

                    // Perform movement
                    mapGenerator.ScrollMap();

                    // Create link between current and touched
                    currentDot.linkedWith = touchedDot;

                    // The touched dot is now the current dot
                    currentDot = touchedDot;
                    CreateLink(currentDot);
                }
                else
                {
                    // If the color is not the same as our current
                    // GameManager.Instance.Harm();
                }

                touchedDot = null;
            }
        }

        private void CreateLink(Dot currentDot)
        {
            // Add a LineRenderer component to the current dot
            LineRenderer line = currentDot.gameObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
            // Configure it
            line.material = currentDot.dotMaterial;
            line.startWidth = lineThickness;
            line.endWidth = lineThickness;
            line.generateLightingData = true;
            // Set its reference in the dot
            currentDot.line = line;
        }
    }
}
