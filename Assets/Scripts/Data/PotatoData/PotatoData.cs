using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PotatoData")]
public class PotatoData : ScriptableObject
{
    public List<Potato> potatoes = new List<Potato>();
    public List<Weapons> weapons = new List<Weapons>();
}

public enum TypeWeapon
{
    Gun,
    Sword
}