using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class Sword : Weapon
{
    [SerializeField] private BoxCollider2D boxMelee;
    
    private readonly PotatoModel _potatoModel = PotatoModel.Instance;
    private Sequence _attack;
    private Vector3 _direction;

    private void Start()
    {
        Init();
        DetectAndAttackTarget();
    }

    protected override void Init()
    {
        base.Init();
        attackSpeed = stats.Find(s => s.statType == StatType.AttackSpeed).baseValue;
        attackRange = stats.Find(s => s.statType == StatType.AttackRange).baseValue;
        detectRange = stats.Find(s => s.statType == StatType.DetectRange).baseValue;
    }
    private void Update()
    {
        _direction = _potatoModel.moveDirection == Vector3.zero
            ? Vector3.right
            : _potatoModel.moveDirection;
        var target = enemyInsideArea.Length <= 0 ? _direction * 100 : targetPosMin;
        LookAtTarget(target, transform);
    }
    
    [Button]
    protected override void Attack()
    {
        base.Attack();
        _attack?.Kill();
        var attack = true;
        boxMelee.enabled = !attack;
        var localPos = transform.localPosition;
        var endValue = new Vector3(0, 0, AngleBetweenPoints(targetPosMin, transform.position));
        _attack = DOTween.Sequence().Append(transform.DORotate(endValue, 0.1f)).AppendCallback(() => boxMelee.enabled = attack).Append(transform.DOMove(targetPosMin, 0.1f)).AppendCallback(() => boxMelee.enabled = !attack)
            .Append(transform.DOLocalMove(localPos, 0.1f));
    }

    protected override void DetectAndAttackTarget()
    {
        base.DetectAndAttackTarget();
        if(enemyInsideArea.Length > 0 && Vector3.Distance(targetPosMin, transform.position) <= attackRange)
            Attack();
    }
}