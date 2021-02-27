using System.Collections;
using UnityEngine;
using DG.Tweening;


namespace DotRun.VFX
{
    public class RingVFX : MonoBehaviour
    {
        [SerializeField] Vector3 scaleTo = new Vector3(2, 2, 1);
        [SerializeField] float duration = 0.5f;

        private void OnEnable()
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.DOScale(scaleTo, duration).SetEase(Ease.InOutCirc);
        }
    }
}
