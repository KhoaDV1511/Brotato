using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UnityEngine;
using UnityEngine.U2D;

public class ShopEquipmentMediator : MonoBehaviour, IEnhancedScrollerDelegate
{
    [SerializeField] private EnhancedScroller scroll;
    [SerializeField] private EnhancedScrollerCellView cellPrefab;
    
    private RateEquipment RateEquipment => GlobalData.Ins.rateEquipment;
    private List<EquipmentItemInfo> _equipmentItemInfos = new List<EquipmentItemInfo>();
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