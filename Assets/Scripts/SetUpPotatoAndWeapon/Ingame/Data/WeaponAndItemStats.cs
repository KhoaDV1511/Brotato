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
    public float[] meleeAndRangedDame;
    public float[] attackSpeed;
    public float[] critDameChange;
    public float[] dameIncrease;
    public float[] attackRange;
    public float[] detectRange;
    public int[] basePrice;

    public float DameAttack(Tire tire, float meleeAndRanged = 0)
    {
        var crit = Random.Range(0f, 1f);
        if (crit.ValueInRange(0, critDameChange[(int)tire]))
        {
            var dameAttack = dame[(int)tire] * dameIncrease[(int)tire];
            return dameAttack + (int)(dameAttack * this.meleeAndRangedDame[(int)tire] * meleeAndRanged);
        }
        return dame[(int)tire] + (int)(dame[(int)tire] * this.meleeAndRangedDame[(int)tire] * meleeAndRanged);
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
    public List<StatItemIncrease> statItemIncreases = new List<StatItemIncrease>();
    public int price;

    public string Description()
    {
        var des = "";
        foreach (var stat in statItemIncreases)
        {
            des += stat.statIncrease < 0 ? "-" : "+";
            switch (stat.statType)
            {
                case StatType.HP:
                    des += $"{stat.statIncrease} max health\n";
                    break;
                case StatType.SpeedVelocity:
                    var strEnemy = stat.increaseFor == IncreaseFor.Enemy ? "Enemy" : "";
                    des += $"{stat.statIncrease} {strEnemy} speed\n";
                    break;
                case StatType.ATK:
                    des += $"{stat.statIncrease} damage\n";
                    break;
                case StatType.DetectRange:
                    des += $"{stat.statIncrease} detect range\n";
                    break;
                case StatType.AttackSpeed:
                    des += $"{stat.statIncrease} attack speed\n";
                    break;
                case StatType.AttackRange:
                    des += $"{stat.statIncrease} attack range\n";
                    break;
                case StatType.HPRegeneration:
                    des += $"{stat.statIncrease} HP Regeneration\n";
                    break;
                case StatType.MeleeAndRangedDame:
                    des += $"{stat.statIncrease} Melee damage\n+{stat.statIncrease} Ranged Dame\n";
                    break;
                case StatType.Dodge:
                    des += $"{stat.statIncrease} dodge\n";
                    break;
                default:
                    break;
            }
        }

        return des;
    }
}
[Serializable]
public class StatItemIncrease
{
    public StatType statType;
    public IncreaseFor increaseFor;
    public float statIncrease;
}

public enum IncreaseFor
{
    Potato,
    Weapon,
    Enemy
}