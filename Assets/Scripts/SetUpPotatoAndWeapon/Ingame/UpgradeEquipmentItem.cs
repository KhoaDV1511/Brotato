using System;
using EnhancedUI.EnhancedScroller;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeEquipmentItem : EnhancedScrollerCellView
{
    [SerializeField] private Image imgEquipment;
    [SerializeField] private TextMeshProUGUI txtDescription, txtPrice;
    [SerializeField] private Button btnEquipment;

    private readonly PotatoModel _potatoModel = PotatoModel.Instance;
    private EquipmentItemInfo _equipmentItemInfo;

    private void Start()
    {
        btnEquipment.onClick.AddListener(OnClickUpgrade);
    }

    public void SetData(EquipmentItemInfo equipmentItemInfo)
    {
        imgEquipment.sprite = equipmentItemInfo.GetSprite();
        txtDescription.SetText(equipmentItemInfo.Description());
        txtPrice.SetText(equipmentItemInfo.Price().ToString());
        
        _equipmentItemInfo = equipmentItemInfo;
    }

    private void OnClickUpgrade()
    {
        if(_potatoModel.dropItemPicked < _equipmentItemInfo.Price()) return;
        _potatoModel.dropItemPicked -= _equipmentItemInfo.Price();
        Signals.Get<RefreshDropPicked>().Dispatch();
        
        BuyItem();
    }

    private void BuyItem()
    {
        if (_equipmentItemInfo.typeEquipment == TypeEquipment.Weapon)
        {
            if(_potatoModel.elementWeaponUpgrades.Count >= PotatoKey.MAX_WEAPON_IN_HAND) return;
            Signals.Get<UpgradeWeaponSignals>().Dispatch(_equipmentItemInfo);
            Signals.Get<BuyItemRollClick>().Dispatch(_equipmentItemInfo.idRoll);
        }
        else
        {
            Signals.Get<UpgradeItemSignals>().Dispatch(_equipmentItemInfo);
            Signals.Get<BuyItemRollClick>().Dispatch(_equipmentItemInfo.idRoll);
        }
    }
}