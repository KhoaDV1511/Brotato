using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.U2D;

public class WeaponMediator : MonoBehaviour
{
    [SerializeField] private Weapon[] weaponGroup;
    [SerializeField] private Color[] color;

    private readonly RenderWeapon _renderWeapon = new RenderWeapon();
    private readonly PotatoModel _potatoModel = PotatoModel.Instance;
    private List<ElementWeaponUpgrade> ElementWeaponUpgrades => _potatoModel.elementWeaponUpgrades;
    private List<WeaponInfo> _weapons => GlobalData.Ins.potatoData.weapons;
    private readonly StartGameSignals _startGameSignals = Signals.Get<StartGameSignals>();
    private readonly StartNewWaveSignals _startNewWaveSignals = Signals.Get<StartNewWaveSignals>();
    
    private void Start()
    {
        ElementStartGame();
    }
    
    private void OnEnable()
    {
        _startGameSignals.AddListener(ElementStartGame);
        _startGameSignals.AddListener(RenderWeaponStartNewWave);
        _startNewWaveSignals.AddListener(RenderWeaponStartNewWave);
    }

    private void OnDisable()
    {
        _startGameSignals.RemoveListener(ElementStartGame);
        _startGameSignals.RemoveListener(RenderWeaponStartNewWave);
        _startNewWaveSignals.RemoveListener(RenderWeaponStartNewWave);
    }
    
    private void ElementStartGame()
    {
        ElementWeaponUpgrades.Clear();
        ElementWeaponUpgrades.Add(new ElementWeaponUpgrade(_potatoModel.weaponId, Tire.TireOne));
    }

    private void RenderWeaponStartNewWave()
    {
        transform.DestroyChildren();
        transform.localPosition =  ElementWeaponUpgrades.Count == 2 ? Vector2.zero : Vector2.up * 0.25f;
        for (var i = 0; i <  ElementWeaponUpgrades.Count; i++)
        {
            var i1 = i;
            this.WaitTimeout(() => SpawnWeapon(i1, ElementWeaponUpgrades.Count, _potatoModel.elementWeaponUpgrades[i1].tire), 0.1f * i);
        }
    }

    private void SpawnWeapon(int i, int quantityWeapon, Tire tire)
    {
        transform.eulerAngles = Vector3.zero;
        var obj = Instantiate(weaponGroup[(int)_weapons[_potatoModel.weaponId - 1].typeWeapon], transform);
        var position = transform.position;
        var angle = 2 * Mathf.PI * i / quantityWeapon + SetUpAngle(quantityWeapon);
        obj.transform.position =
            new Vector2(position.x + SetUpRadius(quantityWeapon) * Mathf.Sin(angle),
                position.y + SetUpRadius(quantityWeapon) * Mathf.Cos(angle));
        obj.SetWeapon(_renderWeapon.GetSprite(_potatoModel.weaponId), color[(int)tire]);
        obj.Init(tire);
    }

    private float SetUpAngle(int quant)
    {
        float zAxis = 0;
        zAxis = quant switch
        {
            1 => Mathf.PI,
            2 => Mathf.PI / 2,
            3 => 0,
            4 => Mathf.PI / 4,
            5 => 0,
            6 => Mathf.PI / 6,
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