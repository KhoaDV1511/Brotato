using System;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public EnemyData enemyData;
    public PotatoData potatoData;

    private static GlobalData _ins;

    private void Awake()
    {
        _ins = this;
    }

    public static GlobalData Ins => _ins;
}