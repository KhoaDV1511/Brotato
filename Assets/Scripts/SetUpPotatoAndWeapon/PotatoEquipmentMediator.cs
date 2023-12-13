using System;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class PotatoEquipmentMediator : MonoBehaviour, IEnhancedScrollerDelegate
{
    [SerializeField] private EnhancedScroller scroll;
    [SerializeField] private EnhancedScrollerCellView cellPotatoPrefab, cellWeaponPrefab;
    [SerializeField] private Button btnNext;
    [SerializeField] private GameObject choosePotato;

    private readonly PotatoModel _potatoModel = PotatoModel.Instance;
    private List<Potato> _potatoes => GlobalData.Ins.potatoData.potatoes;
    private List<Weapons> _weapons = new List<Weapons>();

    private ChooseProgress _chooseProgress;

    private void Awake()
    {
        scroll.Delegate = this;
        _chooseProgress = ChooseProgress.PotatoCharacter;
    }

    private void OnEnable()
    {
        choosePotato.Show();
        _weapons = GlobalData.Ins.potatoData.weapons;
    }

    private void Start()
    {
        scroll.ReloadData();
        btnNext.onClick.AddListener(BtnNextClick);
    }

    private void BtnNextClick()
    {
        if (_chooseProgress == ChooseProgress.PotatoCharacter)
        {
            _chooseProgress = ChooseProgress.Weapon;
            _weapons = _weapons.FindAll(w =>
                w.typeWeapon == _potatoes.Find(p => p.id == _potatoModel.potatoId).typeWeapon);
            scroll.ReloadData();
        }
        else
        {
            gameObject.Hide();
            Signals.Get<StartGameSignals>().Dispatch();
        }
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return _chooseProgress == ChooseProgress.PotatoCharacter ? _potatoes.Count : _weapons.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return 100;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        EnhancedScrollerCellView cell = null;
        switch (_chooseProgress)
        {
            case ChooseProgress.PotatoCharacter:
                cell = scroller.GetCellView(cellPotatoPrefab);
                cell.Cast<PotatoItem>().SetData(_potatoes[dataIndex]);
                break;
            case ChooseProgress.Weapon:
                cell = scroller.GetCellView(cellWeaponPrefab);
                cell.Cast<WeaponItem>().SetData(_weapons[dataIndex]);
                break;
        }
        return cell;
    }
}
[Serializable]
public class Potato
{
    public int id;
    public int rarity;
    private static SpriteAtlas _avatar;
    private static SpriteAtlas Avatar
    {
        get
        {
            if (_avatar == null) _avatar = Resources.Load<SpriteAtlas>("SpriteAtlas/Potatos");
            return _avatar;
        }
    }
    public Sprite GetSprite()
    {
        return Avatar.GetSprite(name);
    }

    public string name;
    public TypeWeapon typeWeapon;
}
[Serializable]
public class Weapons
{
    public int id;
    public int rarity;
    private static SpriteAtlas _sprWeapon;
    private static SpriteAtlas SprWeapon
    {
        get
        {
            if (_sprWeapon == null) _sprWeapon = Resources.Load<SpriteAtlas>("SpriteAtlas/WeaponUI");
            return _sprWeapon;
        }
    }
    public Sprite GetSprite()
    {
        return SprWeapon.GetSprite(name);
    }

    public string name;
    public TypeWeapon typeWeapon;
}
public enum ChooseProgress
{
    PotatoCharacter,
    Weapon
}