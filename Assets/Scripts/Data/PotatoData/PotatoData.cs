using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PotatoData")]
public class PotatoData : ScriptableObject
{
    public List<PotatoInfo> potatoes = new List<PotatoInfo>();
    public List<WeaponInfo> weapons = new List<WeaponInfo>();
}

public enum TypeWeapon
{
    Ranged,
    Melee
}