using System;
using UnityEngine;
using UnityEngine.Serialization;

public class GlobalData : MonoBehaviour
{
    public EnemyData enemyData;
    public PotatoData potatoData;
    public RateEquipment rateEquipment;
    public WeaponAndItemStats weaponAndItemStats;
    public EnemyStats enemyStats;
    public RateDropItems rateDropItems;

    private static GlobalData _ins;

    public static GlobalData Ins
    {
        get
        {
            if (_ins == null) _ins = FindObjectOfType<GlobalData>();
            return _ins;
        }
    }
}