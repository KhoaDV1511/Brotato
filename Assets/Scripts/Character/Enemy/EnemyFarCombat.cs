using UnityEngine;

public class EnemyFarCombat : Enemy
{
    [SerializeField] private Projectile projectile;

    protected override void Attack()
    {
        base.Attack();
        Debug.Log($"Enemy far Attack {DameAttack}");
        var objBullet = Instantiate(projectile, transform.position, Quaternion.identity);
        var targetPos = (enemyDetected.transform.position - objBullet.transform.position).normalized * 10;
        objBullet.InitBullet(5, targetPos, enemyDetected, DameAttack);
        objBullet.Show();
    }
}