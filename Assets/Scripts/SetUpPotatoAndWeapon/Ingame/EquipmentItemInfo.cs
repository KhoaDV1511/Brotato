using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class EquipmentItemInfo
{
    public int idRoll;
    public int idItem;
    public Tire tire;
    public TypeEquipment typeEquipment;
    private Sprite _sprWeapon;

    private List<WeaponStat> _weaponStats => GlobalData.Ins.weaponAndItemStats.weaponStats;
    private WeaponStat _weaponStat => _weaponStats.Find(w => w.id == idItem);
    private List<ItemStat> _itemStats => GlobalData.Ins.weaponAndItemStats.itemStats;
    public ItemStat ItemStat => _itemStats.Find(i => i.id == idItem);

    private Sprite sprWeapon => _sprWeapon.GetSpriteWeapon(idItem);
    public Sprite GetSprite()
    {
        return typeEquipment == TypeEquipment.Weapon ? sprWeapon : SprItem.GetSprite($"Item_{idItem}");
    }

    public int Price()
    {
        return typeEquipment == TypeEquipment.Weapon ? _weaponStat.Price(tire) : ItemStat.price;
    }

    public string Description()
    {
        return typeEquipment == TypeEquipment.Weapon ? DescriptionWeapon() : ItemStat.Description();
    }

    private string DescriptionWeapon()
    {
        return $"Sát thương cơ bản {_weaponStat.DameAttack(tire)}\nTốc độ đánh {_weaponStat.AttackSpeed(tire)}\nTầm đánh {_weaponStat.AttackRange(tire)}";
    }

    public EquipmentItemInfo(int idRoll, int idItem, Tire tire, TypeEquipment typeEquipment)
    {
        this.idRoll = idRoll;
        this.idItem = idItem;
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