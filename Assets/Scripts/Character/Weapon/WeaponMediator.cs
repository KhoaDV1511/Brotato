using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.U2D;

public class WeaponMediator : MonoBehaviour
{
    [SerializeField] private Weapon[] weaponGroup;

    private readonly RenderWeapon _renderWeapon = new RenderWeapon();
    private readonly PotatoModel _potatoModel = PotatoModel.Instance;
    private List<WeaponInfo> _weapons => GlobalData.Ins.potatoData.weapons;
    private readonly StartGameSignals _startGameSignals = Signals.Get<StartGameSignals>();

    public int quantityWeapon = 1;

    private void OnEnable()
    {
        _startGameSignals.AddListener(RenderWeapon);
    }

    private void OnDisable()
    {
        _startGameSignals.RemoveListener(RenderWeapon);
    }

    [Button]
    private void RenderWeapon()
    {
        transform.DestroyChildren();
        transform.localPosition = quantityWeapon == 2 ? Vector2.zero : Vector2.up * 0.25f;
        for (var i = 0; i < quantityWeapon; i++)
        {
            var i1 = i;
            this.WaitTimeout(() => SpawnWeapon(i1), 0.1f * i);
        }
    }

    private void SpawnWeapon(int i)
    {
        transform.eulerAngles = Vector3.zero;
        var obj = Instantiate(weaponGroup[(int)_weapons[_potatoModel.weaponId - 1].typeWeapon], transform);
        var position = transform.position;
        var angle = 2 * Mathf.PI * i / quantityWeapon;
        obj.transform.position =
            new Vector2(position.x + SetUpRadius(quantityWeapon) * Mathf.Sin(angle),
                position.y + SetUpRadius(quantityWeapon) * Mathf.Cos(angle));
        obj.SetWeapon(_renderWeapon.GetSprite(_potatoModel.weaponId));
        transform.eulerAngles = Vector3.forward * SetUpZAxis(quantityWeapon);
    }

    private int SetUpZAxis(int quant)
    {
        var zAxis = 0;
        zAxis = quant switch
        {
            1 => 180,
            2 => 90,
            3 => 0,
            4 => 45,
            5 => 0,
            6 => 30,
            _ => zAxis
        };

        return zAxis;
    }
    private float SetUpRadius(int quant)
    {
        var radius = 0f;
        radius = quant switch
        {
            1 => 0.3f,
            2 => 0.35f,
            3 => 0.4f,
            4 => 0.45f,
            5 => 0.5f,
            6 => 0.55f,
            _ => radius
        };

        return radius;
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