using EnhancedUI.EnhancedScroller;
using UnityEngine;
using UnityEngine.UI;

public class WeaponItem : EnhancedScrollerCellView
{
    [SerializeField] private Image imgWeapon;
    [SerializeField] private Button btnWeapon;

    private readonly PotatoModel _potatoModel = PotatoModel.Instance;

    private int _id;

    private void Start()
    {
        btnWeapon.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        _potatoModel.weaponId = _id;
    }
    public void SetData(Weapons weapons)
    {
        imgWeapon.sprite = weapons.GetSprite();
        _id = weapons.id;
    }
}