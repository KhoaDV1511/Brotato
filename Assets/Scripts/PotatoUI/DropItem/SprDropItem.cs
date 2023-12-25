using System.Net.NetworkInformation;
using DG.Tweening;
using UnityEngine;

public class SprDropItem : MonoBehaviour, DropItemInfo
{
    [SerializeField] private SpriteRenderer sprDropItem;

    private readonly HarvestToStoreSignals _harvestToStoreSignals =
        Signals.Get<HarvestToStoreSignals>();

    private readonly PotatoModel _potatoModel = PotatoModel.Instance;
    private Sequence _dropMove;
    private DurationDrop _durationDrop;
    private void OnEnable()
    {
        _harvestToStoreSignals.AddListener(PickItem);
    }

    private void OnDisable()
    {
        _harvestToStoreSignals.RemoveListener(PickItem);
    }

    private void PickItem()
    {
        if(_durationDrop == DurationDrop.InGame) return;
        Pick(DurationDrop.EndWave);
    }

    private void Pick(DurationDrop durationDrop)
    {
        if (durationDrop == DurationDrop.InGame)
        {
            _potatoModel.dropItemPicked += Level + 1;
            MoveDropItem();
            Signals.Get<PickItemSignals>().Dispatch(Level, transform.position);
            Signals.Get<UpdateDropItemPickedSignals>().Dispatch(Level + 1);
        }
        else
        {
            _potatoModel.dropItemInfos.Add(this);
            Signals.Get<PotatoPickDropItemToStoreSignals>().Dispatch(transform.position);
            Destroy(gameObject);
        }
    }
    
    private void MoveDropItem()
    {
        _dropMove?.Kill();
        _dropMove = DOTween.Sequence().Append(transform.DOLocalMove(Vector3.zero, 0.5f)).Append(transform.DOScale(0, 0.3f).From(1))
            .AppendCallback(() => Destroy(gameObject));
    }
    public void InitDropItem(Sprite sprItem, int level = 0)
    {
        sprDropItem.sprite = sprItem;
        Level = level;
        gameObject.Show();
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(PotatoTag.PLAYER))
        {
            transform.SetParent(col.transform);
            _durationDrop = DurationDrop.InGame;
            Pick(_durationDrop);
        }
    }

    public int Level { get; set; }
}