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
        [SerializeField] Image logo = null;
        [SerializeField] Vector3 endPoint;
        [SerializeField] float duration = 3f;

        IEnumerator Start()
        {
            yield return new WaitForSeconds(1f);
            transform.DOLocalMove(endPoint, duration).SetEase(Ease.OutBounce);
            yield return new WaitForSeconds(duration);
            SceneLoaderManager.Instance.FadeToLevel(Constants.SCENE_INDEX_MAIN_MENU);
        }
    }
}

