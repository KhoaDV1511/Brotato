using System;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShopEquipmentMediator : MonoBehaviour, IEnhancedScrollerDelegate
{
    [SerializeField] private EnhancedScroller scroll;
    [SerializeField] private EnhancedScrollerCellView cellPrefab;

    private readonly PotatoModel _potatoModel = PotatoModel.Instance;
    private readonly BuyItemRollClick _buyItemRollClick = Signals.Get<BuyItemRollClick>();
    private RateEquipment RateEquipment => GlobalData.Ins.rateEquipment;
    private readonly List<EquipmentItemInfo> _equipmentItemInfos = new List<EquipmentItemInfo>();
    private List<ItemStat> _itemStats => GlobalData.Ins.weaponAndItemStats.itemStats;

    private void Awake()
    {
        scroll.Delegate = this;
    }

    private void OnEnable()
    {
        _buyItemRollClick.AddListener(ReloadDataAfterBuy);
    }

    private void OnDisable()
    {
        _buyItemRollClick.RemoveListener(ReloadDataAfterBuy);
    }

    private void ReloadDataAfterBuy(int idRoll)
    {
        _equipmentItemInfos.RemoveAt(_equipmentItemInfos.FindIndex(e => e.idRoll == idRoll));
        scroll.ReloadData();
    }

    private EquipmentItemInfo EquipmentItem(int idRoll, Tire tire)
    {
        var itemStat = _itemStats.FindAll(i => i.tire == tire);
        var itemId = Random.Range(0, itemStat.Count);
        return new EquipmentItemInfo(idRoll, itemStat[itemId].id, tire, TypeEquipment.Item);
    }

    private EquipmentItemInfo EquipmentWeapon(int idRoll, Tire tire)
    {
        return new EquipmentItemInfo(idRoll, _potatoModel.weaponId, tire, TypeEquipment.Weapon);
    }

    public void RollEquipment(int wave)
    {
        _equipmentItemInfos.Clear();
        switch (wave)
        {
            case 1 or 2:
                AddEquipment(i =>
                {
                    _equipmentItemInfos.Add(i is 0 or 1 ? EquipmentWeapon(i, RollTire(wave)) : EquipmentItem(i, RollTire(wave)));
                });
                break;
            case 3:
                AddEquipment(i =>
                {
                    _equipmentItemInfos.Add(i == 0 ? EquipmentWeapon(i, RollTire(wave)) : EquipmentItem(i, RollTire(wave)));
                });
                break;
            case 4 or 5:
                AddEquipment(i =>
                {
                    var typeEquipment = Random.Range((int)TypeEquipment.Weapon, (int)TypeEquipment.Item + 1);
                    _equipmentItemInfos.Add(i == 0 ? EquipmentWeapon(i, RollTire(wave)) : typeEquipment == (int)TypeEquipment.Weapon
                            ? EquipmentWeapon(i, RollTire(wave)) : EquipmentItem(i, RollTire(wave)));
                });
                break;
            default:
                AddEquipment(i =>
                {
                    var typeEquipment = Random.Range(0f, 1f);
                    _equipmentItemInfos.Add(typeEquipment < 0.35 ? EquipmentWeapon(i, RollTire(wave)) : EquipmentItem(i, RollTire(wave)));
                });
                break;
        }

        scroll.ReloadData();
    }

    private void AddEquipment(Action<int> equipmentAdd)
    {
        for (var i = 0; i < 4; i++)
        {
            equipmentAdd.Invoke(i);
        }
    }

    private Tire RollTire(int wave)
    {
        var ratePerWave = RateEquipment.ratePerWaves.Find(r => r.wave == wave);
        var rate = Random.Range(0f, 1f);
        if (rate.ValueInRange(0, ratePerWave.tireOne))
        {
            return Tire.TireOne;
        }
        else if (rate.ValueInRange(ratePerWave.tireOne, ratePerWave.ValueTireTwo))
        {
            return Tire.TireTwo;
        }
        else if (rate.ValueInRange(ratePerWave.ValueTireTwo, ratePerWave.ValueTireThree))
        {
            return Tire.TireThree;
        }
        else
        {
            return Tire.TireFour;
        }
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return _equipmentItemInfos.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return 300;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        var cell = scroller.GetCellView(cellPrefab);
        cell.Cast<UpgradeEquipmentItem>().SetData(_equipmentItemInfos[dataIndex]);
        return cell;
    }
}

public enum TypeEquipment
{
    Weapon,
    Item
}

public enum Tire
{
    TireOne,
    TireTwo,
    TireThree,
    TireFour
}