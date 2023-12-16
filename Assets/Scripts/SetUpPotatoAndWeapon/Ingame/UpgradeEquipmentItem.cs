using System;
using EnhancedUI.EnhancedScroller;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeEquipmentItem : EnhancedScrollerCellView
{
    [SerializeField] private Image imgEquipment;
    [SerializeField] private TextMeshProUGUI txtDescription;
    [SerializeField] private Button btnEquipment;

    private readonly PotatoModel _potatoModel = PotatoModel.Instance;
    private TypeEquipment _typeEquipment;
    private EquipmentItemInfo _equipmentItemInfo;

    private void Start()
    {
        btnEquipment.onClick.AddListener(OnClickUpgrade);
    }

    public void SetData(EquipmentItemInfo equipmentItemInfo)
    {
        imgEquipment.sprite = equipmentItemInfo.GetSprite();
        txtDescription.SetText(equipmentItemInfo.Description());

        _typeEquipment = equipmentItemInfo.typeEquipment;
        _equipmentItemInfo = equipmentItemInfo;
    }

    private void OnClickUpgrade()
    {
        if (_typeEquipment == TypeEquipment.Weapon)
        {
            if(_potatoModel.currentWeaponValue >= PotatoKey.MAX_WEAPON_IN_HAND) return;
            Signals.Get<UpgradeWeaponSignals>().Dispatch(_equipmentItemInfo);
            _potatoModel.currentWeaponValue += 1;
        }
        else
        {
            Signals.Get<UpgradeItemSignals>().Dispatch(_equipmentItemInfo);
        }
    }
}