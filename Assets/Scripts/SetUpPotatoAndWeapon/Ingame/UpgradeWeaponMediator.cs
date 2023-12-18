using System;
using System.Collections.Generic;
using System.Linq;
using EnhancedUI.EnhancedScroller;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeWeaponMediator : MonoBehaviour, IEnhancedScrollerDelegate
{
    [SerializeField] private EnhancedScroller scroll;
    [SerializeField] private EnhancedScrollerCellView cellPrefab;

    private readonly UpgradeWeaponSignals _upgradeWeaponSignals = Signals.Get<UpgradeWeaponSignals>();
    private readonly MergeWeaponSignals _mergeWeaponSignals = Signals.Get<MergeWeaponSignals>();

    private readonly PotatoModel _potatoModel = PotatoModel.Instance;
    private List<ElementWeaponUpgrade> ElementWeaponUpgrades => _potatoModel.elementWeaponUpgrades;

    private void Awake()
    {
        scroll.Delegate = this;
    }


    private void OnEnable()
    {
        _upgradeWeaponSignals.AddListener(AddElement);
        _mergeWeaponSignals.AddListener(MergeWeapon);
    }

    private void OnDisable()
    {
        _upgradeWeaponSignals.RemoveListener(AddElement);
        _mergeWeaponSignals.RemoveListener(MergeWeapon);
    }

    private void MergeWeapon(ElementWeaponUpgrade elementWeaponUpgrade)
    {
        var elmSameTire = ElementWeaponUpgrades.FindAll(e => e.tire == elementWeaponUpgrade.tire);
        if (elmSameTire.Count >= 2)
        {
            _potatoModel.elementWeaponUpgrades.RemoveAt(_potatoModel.elementWeaponUpgrades.FindIndex(e => e.tire == elementWeaponUpgrade.tire));
            _potatoModel.elementWeaponUpgrades.RemoveAt(_potatoModel.elementWeaponUpgrades.FindIndex(e => e.tire == elementWeaponUpgrade.tire));
            _potatoModel.elementWeaponUpgrades.Add(new ElementWeaponUpgrade(elementWeaponUpgrade.id, (Tire)((int)elementWeaponUpgrade.tire + 1)));
            FindWeaponSameTire();
        }
        scroll.ReloadData();
    }

    private void AddElement(EquipmentItemInfo equipmentItemInfo)
    {
        ElementWeaponUpgrades.Add(new ElementWeaponUpgrade(equipmentItemInfo.idItem, equipmentItemInfo.tire));
        scroll.ReloadData();
        FindWeaponSameTire();
        scroll.ReloadData();
    }

    private void FindWeaponSameTire()
    {
        foreach (var elm in Enum.GetValues(typeof(Tire)))
        {
            var elmSameTire = ElementWeaponUpgrades.FindAll(e => e.tire == (Tire)elm);
            if (elmSameTire.Count < 2)
            {
                if (elmSameTire.Count > 0)
                {
                    elmSameTire[0].isMultipleSameTire = false;
                }
            }
            else
            {
                foreach (var e in elmSameTire)
                {
                    e.isMultipleSameTire = true;
                }
            }
        }
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return ElementWeaponUpgrades.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return 100;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        var cell = scroller.GetCellView(cellPrefab);
        cell.Cast<WeaponUpgradeItem>().SetData(ElementWeaponUpgrades[dataIndex]);
        return cell;
    }
}

public class ElementWeaponUpgrade
{
    public int id;
    public Tire tire;
    public bool isMultipleSameTire;
    private Sprite _sprWeapon;
    public Sprite sprWeapon => _sprWeapon.GetSpriteWeapon(id);

    public ElementWeaponUpgrade(int id, Tire tire)
    {
        this.id = id;
        this.tire = tire;
    }
}