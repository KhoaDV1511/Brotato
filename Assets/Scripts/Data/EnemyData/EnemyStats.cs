using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "EnemyStats")]
public class EnemyStats : ScriptableObject
{
    public List<EnemyStat> enemyStats = new List<EnemyStat>();
}
[Serializable]
public class EnemyStat
{
    public EnemyType enemyType;
    public float baseDameAttack;
    public float baseHealth;
    public float attackSpeed;
    public float speedVelocity;
    public float attackRange;
    public float[] detectRange;

    public float DetectRange(TypeDetectRangeEnemy typeDetectRangeEnemy)
    {
        return detectRange[(int)typeDetectRangeEnemy];
    }

    public float Health(int level)
    {
        return baseHealth * level;
    }

    public float DameAttack(int level)
    {
        return baseDameAttack * level;
    }
}

public enum TypeDetectRangeEnemy
{
    InsideCam,
    OutSideCam
}