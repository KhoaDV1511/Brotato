using System;
using Cinemachine.Utility;
using UnityEngine;

public class Gun : Weapon
{
    [SerializeField] private SpriteRenderer sprGun;
    [SerializeField] private Projectile projectile;
    
    private readonly PotatoModel _potatoModel = PotatoModel.Instance;

    private void Start()
    {
        attackSpeed = 1;
        attackRange = 5;
        radius = 5;
        DetectAndAttackTarget();
    }

    private void Update()
    {
        var target = enemyInsideArea.Length <= 0 ? Direction() : targetPosMin;
        LookAtTarget(target, transform);

        Vector3 Direction()
        {
            var position = transform.position;
            return _potatoModel.facingRight ? new Vector3(position.x + 100, position.y) : new Vector3(position.x - 100, position.y);
        }
    }

    protected override void LookAtTarget(Vector3 target, Transform weaponPos)
    {
        base.LookAtTarget(target, weaponPos);
        if(enemyInsideArea.Length <= 0)
        {
            sprGun.flipY = false;
            return;
        }
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