using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "WeaponAndItemStats")]
public class WeaponAndItemStats : ScriptableObject
{
    public List<WeaponStat> weaponStats = new List<WeaponStat>();
    public List<ItemStat> itemStats = new List<ItemStat>();
}

[Serializable]
public class WeaponStat
{
    public int id;
    public float[] dame;
    public float[] rateDamage;
    public float[] attackSpeed;
    public float[] critDameChange;
    public float[] dameIncrease;
    public float[] attackRange;
    public float[] detectRange;
    public int[] basePrice;

    public float DameAttack(Tire tire, int statDame = 0)
    {
        var crit = Random.Range(0f, 1f);
        if (crit.ValueInRange(0, critDameChange[(int)tire]))
        {
            var dameAttack = dame[(int)tire] * dameIncrease[(int)tire];
            return dameAttack + (int)(dameAttack * rateDamage[(int)tire] * statDame);
        }
        return dame[(int)tire] + (int)(dame[(int)tire] * rateDamage[(int)tire] * statDame);
    }

    public float AttackSpeed(Tire tire)
    {
        return attackSpeed[(int)tire];
    }

    public float AttackRange(Tire tire)
    {
        return attackRange[(int)tire];
    }
    
    public float DetectRange(Tire tire)
    {
        return detectRange[(int)tire];
    }

    public int Price(Tire tire)
    {
        return basePrice[(int)tire];
    }
}

[Serializable]
public class ItemStat
{
    public int id;
    public Tire tire;
    public StatType statType;
    public int price;
}