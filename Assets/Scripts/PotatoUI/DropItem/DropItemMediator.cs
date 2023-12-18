using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DropItemMediator : MonoBehaviour
{
    [SerializeField] private PotatoMediator potatoMediator;
    [SerializeField] private RectTransform endPosEndWave;
    [SerializeField] private ImgDropItem imgImgDropItem;
    [SerializeField] private SprDropItem sprImgDropItem;
    [SerializeField] private Sprite[] sprDrop;

    private readonly PotatoModel _potatoModel = PotatoModel.Instance;

    private readonly EnemyDeathSignals _enemyDeathSignals = Signals.Get<EnemyDeathSignals>();
    private readonly PotatoPickDropItemToStoreSignals _potatoPickDropItemSignals = Signals.Get<PotatoPickDropItemToStoreSignals>();

    private void OnEnable()
    {
        _enemyDeathSignals.AddListener(DropSprItem);
        _potatoPickDropItemSignals.AddListener(DropImgItem);
    }

    private void OnDisable()
    {
        _enemyDeathSignals.RemoveListener(DropSprItem);
        _potatoPickDropItemSignals.RemoveListener(DropImgItem);
    }

    private void DropSprItem(Vector2 posDrop)
    {
        var objSprDropItem = Instantiate(sprImgDropItem, posDrop, Quaternion.identity);
        objSprDropItem.InitDropItem(sprDrop[0]);
    }

    private void DropImgItem(Vector2 posDrop)
    {
        // var objImgDropItem = Instantiate(imgImgDropItem, transform);
        // objImgDropItem.transform.position = posDrop;
        // objImgDropItem.InitDropItem(sprDrop[0],  endPosEndWave);
        
        potatoMediator.SetValueLevel();
    }
}

public enum DurationDrop
{
    EndWave, InGame
}