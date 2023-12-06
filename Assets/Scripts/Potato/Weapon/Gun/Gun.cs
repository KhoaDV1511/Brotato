using System;
using UnityEngine;

public class Gun : Weapon
{
    [SerializeField] private SpriteRenderer sprGun;
    [SerializeField] private Projectile projectile;

    private void Start()
    {
        attackSpeed = 1;
        attackRange = 4;
        radius = 4;
        DetectAndAttackTarget();
    }

    private void Update()
    {
        LookAtTarget(targetPosMin, transform);
    }

    protected override void LookAtTarget(Vector3 target, Transform weaponPos)
    {
        base.LookAtTarget(target, weaponPos);
        Flip(target, weaponPos);
    }

    private void Flip(Vector3 target, Transform weaponPos)
    {
        sprGun.flipY = target.x < weaponPos.position.x;
    }

    private void Attack()
    {
        var objBullet = Instantiate(projectile, transform);
        objBullet.target = targetPosMin;
        objBullet.Show();
    }

    protected override void DetectAndAttackTarget()
    {
        base.DetectAndAttackTarget();
        if(enemyInsideArea.Length > 0)
            Attack();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}