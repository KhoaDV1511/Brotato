using System;
using EnhancedUI.EnhancedScroller;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUpgradeItem : EnhancedScrollerCellView
{
    [SerializeField] private Image imgBg, imgWeapon, imgReload;
    [SerializeField] private Button btnMerge;
    [SerializeField] private Color[] color;

    private bool _isMultipleSameTire;
    private ElementWeaponUpgrade _elementWeaponUpgrade;

    private void Start()
    {
        btnMerge.onClick.AddListener(MergeWeapon);
    }

    public void SetData(ElementWeaponUpgrade elementWeaponUpgrade)
    {
        _isMultipleSameTire = elementWeaponUpgrade.isMultipleSameTire;
        _elementWeaponUpgrade = elementWeaponUpgrade;
        
        imgReload.gameObject.SetActive(elementWeaponUpgrade.isMultipleSameTire && elementWeaponUpgrade.tire != Tire.TireFour);
        imgBg.color = color[(int)elementWeaponUpgrade.tire];
        imgWeapon.sprite = elementWeaponUpgrade.sprWeapon;
    }

    private void MergeWeapon()
    {
        if(_isMultipleSameTire && _elementWeaponUpgrade.tire != Tire.TireFour) Signals.Get<MergeWeaponSignals>().Dispatch(_elementWeaponUpgrade);
    }
}