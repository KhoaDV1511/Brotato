using System;
using Cinemachine.Utility;
using UnityEngine;

public class Gun : Weapon
{
    [SerializeField] private Projectile projectile;
    
    private readonly PotatoModel _potatoModel = PotatoModel.Instance;

    private void Start()
    {
        attackSpeed = 1;
        attackRange = 6;
        radius = 7;
        DetectAndAttackTarget();
    }

    private void Update()
    {
        var target = enemyInsideArea.Length <= 0 ? Direction() : targetPosMin;
        LookAtTarget(target, transform);

        Vector3 Direction()
        {
            var position = transform.position;
            return _potatoModel.facingRight ? new Vector3(position.x + 1, position.y) : new Vector3(position.x - 1, position.y);
        }
    }

    protected override void LookAtTarget(Vector3 target, Transform weaponPos)
    {
        base.LookAtTarget(target, weaponPos);
        
        Flip(target, weaponPos);
    }

    private void Flip(Vector3 target, Transform weaponPos)
    {
        transform.localScale = new Vector2 (1, target.x > weaponPos.position.x ? 1 : -1); 
    }

    protected override void Attack()
    {
        base.Attack();
        var objBullet = Instantiate(projectile, transform);
        objBullet.target = targetPosMin;
        objBullet.Show();
    }

    protected override void DetectAndAttackTarget()
    {
        base.DetectAndAttackTarget();
        if(enemyInsideArea.Length > 0 && Vector3.Distance(targetPosMin, transform.position) <= attackRange)
            Attack();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}