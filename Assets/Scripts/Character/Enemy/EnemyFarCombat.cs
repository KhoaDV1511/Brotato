using UnityEngine;

public class EnemyFarCombat : Enemy
{
    [SerializeField] private Projectile projectile;
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
        Debug.Log("Enemy far Attack");
        var objBullet = Instantiate(projectile, transform);
        objBullet.target = (targetPosMin - objBullet.transform.position).normalized * 10;
        objBullet.InitBullet(5);
        objBullet.Show();
    }
}