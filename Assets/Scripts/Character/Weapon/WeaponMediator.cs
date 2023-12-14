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
    private readonly StartGameSignals _startGameSignals = Signals.Get<StartGameSignals>();

    private void OnEnable()
    {
        _startGameSignals.AddListener(RenderWeapon);
    }

    private void OnDisable()
    {
        _startGameSignals.RemoveListener(RenderWeapon);
    }

    private void RenderWeapon()
    {
        var obj = Instantiate(weaponGroup[(int)_weapons[_potatoModel.weaponId - 1].typeWeapon], transform);
        obj.transform.localPosition = Vector3.up * -0.25f;
        obj.SetWeapon(_renderWeapon.GetSprite(_potatoModel.weaponId));
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
        return WeaponAtlas.GetSprite($"{TypeBody.Weapon.ToString()}_{id}");
    }
}