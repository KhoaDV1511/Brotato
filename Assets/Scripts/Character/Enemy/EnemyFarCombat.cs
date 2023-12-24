using UnityEngine;

public class EnemyFarCombat : Enemy
{
    [SerializeField] private Projectile projectile;

    protected override void Attack()
    {
        base.Attack();
        Debug.Log($"Enemy far Attack {DameAttack}");
        var objBullet = Instantiate(projectile, transform.position, Quaternion.identity);
        var position = objBullet.transform.position;
        var targetPos = (enemyDetected.transform.position - position).FindVectorADistanceVecTorB(position, 10);
        objBullet.InitBullet(5, targetPos, enemyDetected, DameAttack);
    }
}