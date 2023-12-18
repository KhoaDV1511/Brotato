using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ImgDropItem : MonoBehaviour
{
    [SerializeField] private RectTransform imgRect;
    [SerializeField] private Image imgDropItem;
    private Sequence _dropMove;
    private readonly PotatoDeathSignals _potatoDeathSignals = Signals.Get<PotatoDeathSignals>();

    private void OnEnable()
    {
        _potatoDeathSignals.AddListener(OnPotatoDeath);
    }

    private void OnDisable()
    {
        _potatoDeathSignals.RemoveListener(OnPotatoDeath);
    }

    private void OnPotatoDeath()
    {
        Destroy(gameObject);
    }
    private void MoveDropItem(RectTransform rect)
    {
        _dropMove?.Kill();
        _dropMove = DOTween.Sequence().Append(imgRect.DOMove(rect.position, 1)).Append(transform.DOScale(0, 0.3f).From(1))
            .AppendCallback(() => Destroy(gameObject));
    }

    public void InitDropItem(Sprite sprItem, RectTransform endPos)
    {
        imgDropItem.sprite = sprItem;
        imgDropItem.SetNativeSize();
        gameObject.Show();
        MoveDropItem(endPos);
    }
}