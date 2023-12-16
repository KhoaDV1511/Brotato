using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class Sword : Weapon
{
    [SerializeField] private BoxCollider2D boxMelee;

    private readonly PotatoModel _potatoModel = PotatoModel.Instance;
    private Sequence _attack;
    private bool _isAttack = true;

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
        _isAttack = !_isAttack;
        boxMelee.enabled = !_isAttack;
    }

    private void Update()
    {
        if (_isAttack) return;
        LookAtTargetAndFlip(transform);
    }

    [Button]
    protected override void Attack()
    {
        base.Attack();
        _attack?.Kill();
        var transform1 = transform;
        var localPos = transform1.localPosition;
        var position = transform1.position;
        var endValue = new Vector3(0, 0, AngleBetweenPoints(targetPosMin, position));

        _attack = DOTween.Sequence().Append(transform.DOLocalRotate(endValue, 0.1f))
            .AppendCallback(() =>
            {
                _isAttack = true;
                boxMelee.enabled = _isAttack;
            }).Append(transform.DOLocalMove((targetPosMin - position).FindVectorADistanceVecTorB(localPos, attackRange),
                0.1f))
            .AppendCallback(() => boxMelee.enabled = false).AppendInterval(0.1f)
            .Append(transform.DOLocalMove(localPos, 0.1f)).AppendCallback(() => _isAttack = false);
    }

    protected override void DetectAndAttackTarget()
    {
        base.DetectAndAttackTarget();
        if (enemyInsideArea.Length > 0 && Vector3.Distance(targetPosMin, transform.position) <= attackRange)
            Attack();
    }
}