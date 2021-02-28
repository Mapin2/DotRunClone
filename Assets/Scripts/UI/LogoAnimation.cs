using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DotRun.Core;
using DotRun.Utils;

namespace DotRun.UI
{
    public class LogoAnimation : MonoBehaviour
    {
        [SerializeField] private Image logo = null;
        [SerializeField] private Vector3 endPoint = new Vector3();
        [SerializeField] private float duration = 3f;

        IEnumerator Start()
        {
            yield return new WaitForSeconds(1f);
            transform.DOLocalMove(endPoint, duration).SetEase(Ease.OutBounce);
            yield return new WaitForSeconds(duration);
            SceneLoaderManager.Instance.FadeToLevel(Constants.SCENE_INDEX_MAIN_MENU);
        }
    }
}

