using UnityEngine;
using UnityEngine.U2D;

public class EquipmentItemInfo
{
    public int id;
    public Tire tire;
    public TypeEquipment typeEquipment;
    private Sprite _sprWeapon;
    private Sprite sprWeapon => _sprWeapon.GetSpriteWeapon(id);
    public Sprite GetSprite()
    {
        return typeEquipment == TypeEquipment.Weapon ? sprWeapon : SprItem.GetSprite($"Item_1");
    }

    public string Description()
    {
        return tire.ToString();
    }

    public EquipmentItemInfo(int id, Tire tire, TypeEquipment typeEquipment)
    {
        this.id = id;
        this.tire = tire;
        this.typeEquipment = typeEquipment;
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
}