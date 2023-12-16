using EnhancedUI.EnhancedScroller;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUpgradeItem : EnhancedScrollerCellView
{
    [SerializeField] private Image imgBg, imgWeapon;
    [SerializeField] private Color[] color;

    public void SetData(ElementWeaponUpgrade elementWeaponUpgrade)
    {
        imgBg.color = color[(int)elementWeaponUpgrade.tire];
        imgWeapon.sprite = elementWeaponUpgrade.sprWeapon;
    }
}