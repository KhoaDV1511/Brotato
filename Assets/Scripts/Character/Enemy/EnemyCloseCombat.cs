using System;
using UnityEngine;

public class EnemyCloseCombat : Enemy
{
    protected override void Attack()
    {
        base.Attack();
        Debug.Log($"Enemy close Attack {DameAttack}");
        if(enemyDetected)
            enemyDetected.ReceiveDamage(StatType.HP, DameAttack);
    }
}