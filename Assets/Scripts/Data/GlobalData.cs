using System;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public EnemyData enemyData;
    public PotatoData potatoData;
    public RateEquipment rateEquipment;

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