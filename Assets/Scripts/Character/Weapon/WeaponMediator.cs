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
    private List<Weapons> _weapons => GlobalData.Ins.potatoData.weapons;
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

        for (var i = 0; i < quantityWeapon; i++)
        {
            var obj = Instantiate(weaponGroup[(int)_weapons[_potatoModel.weaponId - 1].typeWeapon], transform);
            var localPosition = transform.localPosition;
            float angle = 0;
            float raius = 0.7f;
            if (quantityWeapon == 1)
            {
                angle = 180f;
                raius = 0.3f;
            }else if (quantityWeapon == 2)
            {
                angle = 2 * Mathf.PI * i / quantityWeapon + 90;
                raius = 0.5f;
            }
            else angle = 2 * Mathf.PI * i / quantityWeapon;
            obj.transform.localPosition =
                new Vector2(localPosition.x + raius * Mathf.Sin(angle),
                    localPosition.y + raius * Mathf.Cos(angle));
            obj.SetWeapon(_renderWeapon.GetSprite(_potatoModel.weaponId));
        }
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