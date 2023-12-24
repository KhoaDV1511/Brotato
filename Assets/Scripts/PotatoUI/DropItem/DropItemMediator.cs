using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DropItemMediator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtPickItem;
    [SerializeField] private RectTransform endPosEndWave;
    [SerializeField] private ImgDropItem imgImgDropItem;
    [SerializeField] private SprDropItem sprImgDropItem;
    [SerializeField] private Sprite[] sprDrop;

    private readonly PotatoModel _potatoModel = PotatoModel.Instance;
    private List<ElementRateDropItem> ElementRateDropItems => GlobalData.Ins.rateDropItems.elementRateDropItems;

    private readonly EnemyDeathSignals _enemyDeathSignals = Signals.Get<EnemyDeathSignals>();
    private readonly PotatoPickDropItemToStoreSignals _potatoPickDropItemSignals = Signals.Get<PotatoPickDropItemToStoreSignals>();
    private readonly PickItemSignals _pickItemSignals = Signals.Get<PickItemSignals>();

    private Tween _txtXnShow;

    private void OnEnable()
    {
        _enemyDeathSignals.AddListener(DropSprItem);
        _potatoPickDropItemSignals.AddListener(DropImgItem);
        _pickItemSignals.AddListener(PickItemXn);
    }

    private void OnDisable()
    {
        _enemyDeathSignals.RemoveListener(DropSprItem);
        _potatoPickDropItemSignals.RemoveListener(DropImgItem);
        _pickItemSignals.RemoveListener(PickItemXn);
    }

    private void PickItemXn(int Xn, Vector2 posTxtAppear)
    {
        if(Xn < 1) return;
        var objTxt = Instantiate(txtPickItem, transform);
        objTxt.transform.position = posTxtAppear;
        objTxt.SetText($"X{(Xn + 1)}");
        objTxt.Show();
        
        _txtXnShow?.Kill();
        _txtXnShow = DOVirtual.DelayedCall(0.3f, () => objTxt.Hide());
    }

    private void DropSprItem(Vector2 posDrop)
    {
        var dropItem = Random.Range(0f, 1f);
        if(dropItem >= ElementRateDropItems.Find(e => e.wave == _potatoModel.currentWave).dropChange) return;
        var objSprDropItem = Instantiate(sprImgDropItem, posDrop, Quaternion.identity);
        if(_potatoModel.dropItemInfos.Count <= 0)
            objSprDropItem.InitDropItem(sprDrop[0]);
        else
        {
            _potatoModel.dropItemInfos[^1].Level += 1;
            objSprDropItem.InitDropItem(
                _potatoModel.dropItemInfos[^1].Level > sprDrop.Length - 1
                    ? sprDrop[^1]
                    : sprDrop[_potatoModel.dropItemInfos[^1].Level], _potatoModel.dropItemInfos[^1].Level);
            _potatoModel.dropItemInfos.RemoveAt(_potatoModel.dropItemInfos.Count - 1);
        }
    }

    private void DropImgItem(Vector2 posDrop)
    {
        var objImgDropItem = Instantiate(imgImgDropItem, transform);
        objImgDropItem.transform.position = posDrop;
        objImgDropItem.InitDropItem(sprDrop[0],  endPosEndWave);
        
        Signals.Get<RefreshDropPicked>().Dispatch();
    }
}

public enum DurationDrop
{
    EndWave, InGame
}