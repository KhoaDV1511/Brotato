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
    private readonly StartGameSignals _startGameSignals = Signals.Get<StartGameSignals>();

    private PotatoModel _potatoModel = PotatoModel.Instance;

    public List<ElementWeaponUpgrade> elementWeaponUpgrades = new List<ElementWeaponUpgrade>();


    private void Awake()
    {
        scroll.Delegate = this;
    }

    private void Start()
    {
        ElementStartGame();
    }

    private void OnEnable()
    {
        _startGameSignals.AddListener(ElementStartGame);
        _upgradeWeaponSignals.AddListener(AddElement);
    }

    private void OnDisable()
    {
        _startGameSignals.RemoveListener(ElementStartGame);
        _upgradeWeaponSignals.RemoveListener(AddElement);
    }

    private void ElementStartGame()
    {
        elementWeaponUpgrades.Clear();
        elementWeaponUpgrades.Add(new ElementWeaponUpgrade(_potatoModel.weaponId, Tire.TireOne));
        scroll.ReloadData();
    }

    private void AddElement(EquipmentItemInfo equipmentItemInfo)
    {
        elementWeaponUpgrades.Add(new ElementWeaponUpgrade(equipmentItemInfo.id, equipmentItemInfo.tire));
        scroll.ReloadData();
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return elementWeaponUpgrades.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return 100;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        var cell = scroller.GetCellView(cellPrefab);
        cell.Cast<WeaponUpgradeItem>().SetData(elementWeaponUpgrades[dataIndex]);
        return cell;
    }
}

public class ElementWeaponUpgrade
{
    public int id;
    public Tire tire;
    private Sprite _sprWeapon;
    public Sprite sprWeapon => _sprWeapon.GetSpriteWeapon(id);

    public ElementWeaponUpgrade(int id, Tire tire)
    {
        this.id = id;
        this.tire = tire;
    }
}