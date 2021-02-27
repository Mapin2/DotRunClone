using UnityEngine;
using DotRun.Utils;
using DotRun.Core;
using System;

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

                if (hit && hit.gameObject.layer == Constants.LAYER_INTERACTABLE_DOT)
                    hit.TryGetComponent(out touchedDot);
            }
        }

        private void FirstDotTouched()
        {
            if (!currentDot && touchedDot)
            {
                // If it is the same material or a change material type create first link with line renderer
                if (touchedDot.dotMaterial == GameManager.Instance.currentMaterial)
                {
                    currentDot = touchedDot;
                    touchedDot = null;

                    DotActions(currentDot);

                    CreateLink(currentDot);
                
                    GameManager.Instance.StartGame();
                }
                else
                {
                    DotTouchedError();
                }

            }
        }

        private void LinkDots()
        {
            if (touchedDot && GameManager.Instance.gameIsRunning)
            {
                // If it is the same material or a change material type create first link with line renderer
                if (touchedDot.dotMaterial == currentDot.dotMaterial || touchedDot.type == InteractableType.Triangle)
                {
                    if (touchedDot.type == InteractableType.Triangle)
                        mapGenerator.ApplyCurrentColorChange();

                    DotActions(touchedDot);

                    // Create link between current and touched
                    currentDot.linkedWith = touchedDot;

                    // The touched dot is now the current dot
                    currentDot = touchedDot;
                    CreateLink(currentDot);
                }
                else
                {
                    DotTouchedError();
                }

                touchedDot = null;
            }
        }

        private void DotActions(Dot dot)
        {
            // SFX
            switch (dot.type)
            {
                case InteractableType.Dot:
                    dot.PlaySound(SoundType.Normal);
                    break;
                case InteractableType.Triangle:
                    dot.PlaySound(SoundType.ChangeColor);
                    break;
                case InteractableType.PowerUp:
                    dot.PlaySound(SoundType.ChangeColor);
                    break;
            }

            // VFX
            ActivateDotVFX(dot);

            // Activate power ups
            if (dot.type == InteractableType.PowerUp)
                TriggerPowerUp(dot);

            // Save static info about the touched dot
            Dot.latestTouchedDotMaterial = dot.dotMaterial;

            // Perform movement
            mapGenerator.ScrollMap();

            // Add score and gain time
            GameManager.Instance.ScorePoints(dot.points, dot.dotTimeGain);
        }

        private void TriggerPowerUp(Dot dot)
        {
            PowerUp powerUp = dot.GetComponentInChildren<PowerUp>();
            powerUp.Trigger();
        }

        private void DotTouchedError()
        {
            // SFX
            touchedDot.PlaySound(SoundType.Hurt);

            ActivateDotVFX(touchedDot);

            // Save static info about the touched dot
            Dot.latestTouchedDotMaterial = touchedDot.dotMaterial;
            touchedDot = null;

            GameManager.Instance.Hurt();
        }

        private static void ActivateDotVFX(Dot dot)
        {
            // Activate VFX
            ParticleSystem ringVFX = dot.ringVFX;
            ParticleSystem.MainModule ringVFXMainModule = ringVFX.main;
            ringVFXMainModule.startColor = dot.dotMaterial.color;
            ringVFX.gameObject.SetActive(true);
        }

        private void CreateLink(Dot currentDot)
        {
            LineRenderer line = null;
            if (!currentDot.TryGetComponent(out line))
                // Add a LineRenderer component to the current dot
                line = currentDot.gameObject.AddComponent<LineRenderer>();

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
