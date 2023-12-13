using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class Sword : Weapon
{
    private readonly PotatoModel _potatoModel = PotatoModel.Instance;
    private Sequence _attack;
    private Vector3 _direction;

    private void Start()
    {
        attackSpeed = 1;
        attackRange = 5;
        radius = 7;
        
        DetectAndAttackTarget();
    }

    private void Update()
    {
        _direction = _potatoModel.moveDirection == Vector3.zero
            ? Vector3.right
            : _potatoModel.moveDirection * 100;
        var target = enemyInsideArea.Length <= 0 ? _direction : targetPosMin;
        LookAtTarget(target, transform);
    }
    
    [Button]
    protected override void Attack()
    {
        base.Attack();
        _attack?.Kill();
        var endValue = new Vector3(0, 0, AngleBetweenPoints(targetPosMin, transform.position));
        _attack = DOTween.Sequence().Append(transform.DORotate(endValue, 0.1f)).Append(transform.DOMove(targetPosMin, 0.2f))
            .Append(transform.DOLocalMove(Vector3.zero, 0.1f));
    }

    protected override void DetectAndAttackTarget()
    {
        base.DetectAndAttackTarget();
        if(enemyInsideArea.Length > 0 && Vector3.Distance(targetPosMin, transform.position) <= attackRange)
            Attack();
    }
}