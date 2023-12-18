using System.Collections.Generic;
using UnityEngine;

public class PotatoModel : Singleton<PotatoModel>
{
    public int levelPotato;
    public int experienceCurrentPotato;
    public int experienceMaxPerLevel => (int)Mathf.Pow(levelPotato + 4, 2);

    public int dropItemPicked;

    public int dropItemStore;
    // position potato move and face potato
    public Vector3 potatoPos;
    public bool facingRight = true;
    public Vector3 moveDirection = Vector3.zero;

    // id choose wepon and potato
    public int potatoId;
    public int weaponId;

    public readonly List<ElementWeaponUpgrade> elementWeaponUpgrades = new List<ElementWeaponUpgrade>();
}