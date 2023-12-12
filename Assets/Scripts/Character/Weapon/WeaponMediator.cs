using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.U2D;

public class WeaponMediator : MonoBehaviour
{
    [SerializeField] private Weapon[] weaponGroup;

    private readonly RenderWeapon _renderWeapon = new RenderWeapon();
    private readonly PotatoModel _potatoModel = PotatoModel.Instance;
    private List<Weapons> _weapons => GlobalData.Ins.potatoData.weapons;
    private readonly RenderPotatoSignals _renderPotatoSignals = Signals.Get<RenderPotatoSignals>();

    private void OnEnable()
    {
        _renderPotatoSignals.AddListener(RenderWeapon);
    }

    private void OnDisable()
    {
        _renderPotatoSignals.RemoveListener(RenderWeapon);
    }

    private void RenderWeapon()
    {
        var obj = Instantiate(weaponGroup[(int)_weapons[_potatoModel.weaponId].typeWeapon].gameObject, transform);
        obj.transform.position = new Vector3(0, 0.1f, 0);
        weaponGroup[(int)_weapons[_potatoModel.weaponId].typeWeapon].SetWeapon(_renderWeapon.GetSprite(_potatoModel.weaponId));
    }
}

public class RenderWeapon
{
    private static SpriteAtlas _weaponAtlas;
    private static SpriteAtlas WeaponAtlas
    {
        get
        {
            if (_weaponAtlas == null) _weaponAtlas = Resources.Load<SpriteAtlas>($"SpriteAtlas/WeaponInGame");

            return _weaponAtlas;
        }
    }

    public Sprite GetSprite(int id)
    {
        Debug.Log($"{TypeBody.Weapon.ToString()}_{id}");
        return WeaponAtlas.GetSprite($"{TypeBody.Weapon.ToString()}_{id}");
    }
}