using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyData")]
public class EnemyData : ScriptableObject
{
    public List<WaveEnemy> waveEnemies = new List<WaveEnemy>();
    public List<TimePerWave> timePerWaves = new List<TimePerWave>();
    public TextAsset data;

    // [ContextMenu("LoadData")]
    // public void ReadData()
    // {
    //     waveEnemies.Clear();
    //     var jarray = JArray.Parse(data.text);
    //     foreach (var j in jarray)
    //     {
    //         Debug.Log(j);
    //         var turn = int.Parse(j["Turn"]!.ToString());
    //         var turnReloadBase = float.Parse(j["Turn Reload Base"]!.ToString().Replace(",", "."));
    //         var enemyQuantBase = int.Parse(j["Enemy Quant Base"]!.ToString());
    //         waveEnemies.Add(new WaveEnemy(turn, turnReloadBase, enemyQuantBase));
    //     }
    //     
    // }
}

[Serializable]
public class WaveEnemy
{
    public int turn;
    public float turnReloadBase;
    public int enemyQuantBase;
    public EnemyAttribute enemyAttribute;
    public WaveEnemy(int turn, float turnReloadBase, int enemyQuantBase)
    {
        this.turn = turn;
        this.turnReloadBase = turnReloadBase;
        this.enemyQuantBase = enemyQuantBase;
    }
}

[Serializable]
public class TimePerWave
{
    public int wave;
    public int time;

    public TimePerWave(int wave, int time)
    {
        this.wave = wave;
        this.time = time;
    }
}

[Serializable]
public class EnemyAttribute
{
    public int id;
    public string name;
    public Sprite avatar;
    public EnemyType enemyType;
    public int level;

    public int LevelEnemyPerWave(int wave, int turn)
    {
        return level + wave + turn;
    }
}

public enum EnemyType
{
    EnemyCloseCombat,
    EnemyFarCombat
}