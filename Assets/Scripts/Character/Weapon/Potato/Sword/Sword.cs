using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class Sword : Weapon
{
    private readonly PotatoModel _potatoModel = PotatoModel.Instance;

    private void Start()
    {
        attackSpeed = 1;
        attackRange = 5;
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
    
    [Button]
    protected override void Attack()
    {
        base.Attack();
        DOTween.Sequence().Join(transform.DOMove(targetPosMin, 0.3f))
            .Append(transform.DOLocalMove(Vector3.zero, 0.2f));
    }

    protected override void DetectAndAttackTarget()
    {
        base.DetectAndAttackTarget();
        if(enemyInsideArea.Length > 0 && Vector3.Distance(targetPosMin, transform.position) <= attackRange)
            Attack();
    }
}