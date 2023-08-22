using System;
using UnityEngine;
using DotRun.Utils;
using DotRun.Core;
using DG.Tweening;

namespace DotRun.GamePlay
{
    public class Dot : MonoBehaviour
    {

        // Interaction Settings
        private float minInteractablePos = -1.5f; // The min Y position in which the dot will become interactable
        private float maxInteractablePosition = -2.5f; // The max Y position in which the dot will stop being interactable
        public int points = 10; // Points that the dot rewards for touching it correctly
        public float dotTimeGain = 0.5f; // The amount of time that the dot will gain
        public InteractableType type = InteractableType.Dot;

        [Header("Materials Settings")]
        [Tooltip("Material of the current dot")]
        public Material dotMaterial = null;

        [Header("Line Settings")]
        [Tooltip("Line renderer for the dot")]
        public LineRenderer line = null;
        [Tooltip("Dot to which is linked the current dot line renderer")]
        public Dot linkedWith = null;
        
        [Header("VFX Settings")]
        [Tooltip("Child ring vfx")]
        public ParticleSystem ringVFX = null;

        // Movement Settings
        private float movementDuration = 1f; // Duration of the dot down movement
        private float unitsToMove = 1.5f; // Number of units the dot will move down
        private float yPosLimit = -7f; // Y position in which the dot will destroy itself

        [Header("PowerUps")]
        public PowerUp[] powerUps = null;
        
        private MapGenerator mapGenerator = null;

        // Static info to save last dot material touched (used on the game over)
        public static Material latestTouchedDotMaterial = null;

        private void Awake()
        {
            if (!mapGenerator)
                mapGenerator = FindObjectOfType<MapGenerator>();

            // Movement event subscription
            mapGenerator.OnDotTouched += DotMove;
        }

        private void Start()
        {
            // We need the shared material and not the instanced material as we use it for comparing reasons
            dotMaterial = GetComponent<SpriteRenderer>().sharedMaterial;
        }

        private void Update()
        {
            // Become interactable between  Y <= -1 and Y >= -2, and if its not a change current material dot
            gameObject.layer = (transform.position.y <= minInteractablePos && transform.position.y >= maxInteractablePosition) ? Constants.LAYER_INTERACTABLE_DOT : Constants.LAYER_NOT_INTERACTABLE_DOT;

            // If this dot has a LineRenderer update its start position to self position
            if (line)
            {
                line.SetPosition(0, transform.position);

                // If we have a LineRenderer but not linked dot, update its end position to self position too
                if (!linkedWith)
                    line.SetPosition(1, transform.position);
            }

            // Update its end position with the linked dot position
            if (linkedWith)
                line.SetPosition(1, linkedWith.transform.position);

            // Remove (probably should implement a pool)
            if (transform.position.y <= yPosLimit)
            {
                // Movement event unsubscription
                mapGenerator.OnDotTouched -= DotMove;
                // Lost powerUp
                if (type.Equals(InteractableType.PowerUp))
                    PowerUpManager.Instance.powerUpSpawned = false;
                Destroy(gameObject);
            }
        }

        public void DotMove()
        {
            float movement = transform.position.y - unitsToMove;
            transform.DOMoveY(movement, movementDuration).SetEase(Ease.OutQuint);
        }
    }
}
