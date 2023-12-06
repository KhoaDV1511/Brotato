using UnityEngine;
using Random = System.Random;
using Vector3 = System.Numerics.Vector3;

public class CloseCombat : Weapon
{
    private void Start()
    {
        radius = 9.5f;
        attackSpeed = 1;
        attackRange = 0.7f;
        
        DetectAndAttackTarget();
    }
    
    protected override void DetectAndAttackTarget()
    {
        base.DetectAndAttackTarget();
        Attack();
    }

    private void Attack()
    {
        
    }
}
