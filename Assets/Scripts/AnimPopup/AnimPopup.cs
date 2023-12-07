using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class AnimPopup : MonoBehaviour
{
    [SerializeField] private RectTransform content;
    [SerializeField] private CanvasGroup canvasGroup;
    private Sequence _mainSequence;
    public float startSize = 0.7f, middleSize = 1.05f, endSize = 1;
    public float firstTime = 0.2f, secondTime = 0.1f;
    
    private void OnValidate()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    private void OnEnable()
    {
        _mainSequence?.Kill();
        canvasGroup.blocksRaycasts = true;
        _mainSequence = DOTween.Sequence();

        content.localScale = Vector3.one * startSize;
        canvasGroup.alpha = 0;
        _mainSequence.Append(content.DOScale(middleSize, firstTime))
            .Join(canvasGroup.DOFade(1, firstTime))
            .Append(content.DOScale(endSize, secondTime));
    }
}