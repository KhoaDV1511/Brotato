using UnityEngine;
using UnityEngine.U2D;

public class EquipmentItemInfo
{
    public int id;
    public Tire tire;
    public TypeEquipment typeEquipment;
    private static SpriteAtlas _sprWeapon;
    private static SpriteAtlas SprWeapon
    {
        get
        {
            if (_sprWeapon == null) _sprWeapon = Resources.Load<SpriteAtlas>("SpriteAtlas/WeaponUI");
            return _sprWeapon;
        }
    }

    private static SpriteAtlas _sprItem;
    private static SpriteAtlas SprItem
    {
        get
        {
            if (_sprItem == null) _sprItem = Resources.Load<SpriteAtlas>("SpriteAtlas/ItemUI");
            return _sprItem;
        }
    }
    public Sprite GetSprite()
    {
        return typeEquipment == TypeEquipment.Weapon ? SprWeapon.GetSprite($"Weapon_{id}") : SprItem.GetSprite($"Item_1");
    }

    public string Description()
    {
        return tire.ToString();
    }
}