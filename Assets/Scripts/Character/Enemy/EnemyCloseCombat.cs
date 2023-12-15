using System;
using UnityEngine;

public class EnemyCloseCombat : Enemy
{
    protected override void Start()
    {
        base.Start();
        DetectAndAttackTarget();
    }

    protected override void DetectAndAttackTarget()
    {
        base.DetectAndAttackTarget();
        if(enemyInsideArea.Length > 0 && Vector3.Distance(targetPosMin, transform.position) <= attackRange)
            Attack();
    }

    protected override void Attack()
    {
        base.Attack();
        Debug.Log("Enemy close Attack");
    }
}