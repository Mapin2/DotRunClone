using UnityEngine;
using DotRun.Utils;
using DG.Tweening;

namespace DotRun.GamePlay
{
    public class Dot : MonoBehaviour
    {
        [Header("Interaction Settings")]
        [Tooltip("The min Y position in which the dot will become interactable")]
        [SerializeField] private float minInteractablePos = -1f;
        [Tooltip("The max Y position in which the dot will stop being interactable")]
        [SerializeField] private float maxInteractablePosition = -2f;
        [Tooltip("Points that the dot rewards for touching it correctly")]
        [SerializeField] private int points = 10;

        [Header("Materials Settings")]
        [Tooltip("Material of the current dot")]
        public Material dotMaterial = null;

        [Header("Line Settings")]
        [Tooltip("Line renderer for the dot")]
        public LineRenderer line = null;
        [Tooltip("Dot to which is linked the current dot line renderer")]
        public Dot linkedWith = null;

        [Header("Movement Settings")]
        [Tooltip("Duration of the dot down movement")]
        [SerializeField] private float movementDuration = 1f;
        [Tooltip("Number of units the dot will move down")]
        [SerializeField] private float unitsToMove = 2f;
        [Tooltip("Number of units the dot will move down when the Y position goes further than the interactable position")]
        [SerializeField] private float unitsToMoveVariation = 1.5f;
        [Tooltip("Y position in which the dot will destroy itself")]
        [SerializeField] private float yPosLimit = -7f;

        private MapGenerator mapGenerator = null;

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
            if (gameObject.layer != Constants.LAYER_CHANGE_CURRENT_MATERIAL)
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
                Destroy(gameObject);
            }
        }

        public void DotMove()
        {
            // Movement variation based on the dot y position
            float movement = transform.position.y;
            movement = movement <= minInteractablePos ? movement -= unitsToMoveVariation : movement -= unitsToMove;

            transform.DOMoveY(movement, movementDuration).SetEase(Ease.OutQuint);
        }
    }
}
