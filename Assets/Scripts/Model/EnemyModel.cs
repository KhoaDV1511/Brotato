using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class EnemyModel : Singleton<EnemyModel>
{
    public List<WaveEnemy> WaveEnemies => GlobalData.Ins.enemyData.waveEnemies;
    public List<TimePerWave> TimePerWaves => GlobalData.Ins.enemyData.timePerWaves;

    public float TurnReload(int turnIndex, int waveIndex)
    {
        return (float)(WaveEnemies[turnIndex].turnReloadBase
                       * Math.Pow(0.95f, TimePerWaves[waveIndex].wave - 1));
    }

    public int EnemyQuantity(int turnIndex, int waveIndex)
    {
        return Mathf.CeilToInt((float)(WaveEnemies[turnIndex].enemyQuantBase * Math.Pow(1.05f, TimePerWaves[waveIndex].wave - 1)));
    }
}