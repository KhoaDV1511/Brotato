using System;
using DG.Tweening;
using UnityEngine;

public class Sword : Weapon
{
    [SerializeField] private BoxCollider2D boxMelee;
    
    private Sequence _attack;
    private bool _isAttack = true;

    private void OnEnable()
    {
        _isAttack = false;
        boxMelee.enabled = _isAttack;
    }

    private void Update()
    {
        if (_isAttack) return;
        LookAtTargetAndFlip(transform);
    }
    
    protected override void Attack()
    {
        base.Attack();
        //Debug.Log($"Stats weapon dame: {dame}");
        _attack?.Kill();
        var transform1 = transform;
        var localPos = transform1.localPosition;
        var position = transform1.position;
        var enemyPos = enemyDetected.transform.position;
        var endValue = new Vector3(0, 0, AngleBetweenPoints(enemyPos, position));

        _attack = DOTween.Sequence().Append(transform.DOLocalRotate(endValue, 0.1f))
            .AppendCallback(() =>
            {
                _isAttack = true;
                boxMelee.enabled = _isAttack;
            }).Append(transform.DOLocalMove((enemyPos - position).FindVectorADistanceVecTorB(localPos, AttackRange),
                0.1f))
            .AppendCallback(() => boxMelee.enabled = false).AppendInterval(0.1f)
            .Append(transform.DOLocalMove(localPos, 0.1f)).AppendCallback(() => _isAttack = false);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(PotatoTag.ENEMY))
        {
            if(enemyDetected)
            {
                enemyDetected.ReceiveDamage(StatType.HP, DameAttack);
            }
            if(!Equals(col.GetComponent<Character>(), enemyDetected))
            {
                col.GetComponent<Character>().ReceiveDamage(StatType.HP, DameAttack * 0.7f);
            }
        }
    }
}