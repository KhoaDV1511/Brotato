using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] LayerMask mask;
    protected Collider2D[] enemyInsideArea;
    protected Character enemyDetected;
    protected Vector3 enemyPosMin;
    private Coroutine _sweep;
    private readonly List<Character> _enemyPos = new List<Character>();
    private readonly List<float> _distanceEnemy = new List<float>();
    
    protected readonly List<StatCharacter> stats = new List<StatCharacter>();

    private bool _attackCountDown = false;
    protected float statIncrease = 0;
    protected float DameAttack => stats.Find(s => s.statType == StatType.ATK).CurrentValue();
    protected float CurrentHp => stats.Find(s => s.statType == StatType.HP).CurrentValue();
    private float AttackSpeed => stats.Find(s => s.statType == StatType.AttackSpeed).CurrentValue();
    protected float AttackRange => stats.Find(s => s.statType == StatType.AttackRange).CurrentValue();
    protected float DetectRange => stats.Find(s => s.statType == StatType.DetectRange).CurrentValue();
    protected float SpeedVelocity => stats.Find(s => s.statType == StatType.SpeedVelocity).CurrentValue();

    protected virtual void Start()
    {
        DetectAndAttackTarget();
    }

    protected virtual void Attack()
    {

    }

    public virtual void ReceiveDamage(StatType statType, float statIncrease)
    {
        this.statIncrease += statIncrease;
        stats.Find(s => s.statType == statType).statIncrease = -this.statIncrease;
    }

    private void DetectAndAttackTarget()
    {
        if(_sweep != null) StopCoroutine(_sweep);
        _sweep = StartCoroutine(TargetPos());
    }
    private IEnumerator TargetPos()
    {
        _enemyPos.Clear();
        // ReSharper disable once Unity.PreferNonAllocApi
        enemyInsideArea = Physics2D.OverlapCircleAll(transform.position, DetectRange, mask);
        if(enemyInsideArea.Length > 0)
            PosTargetMin();
        
        yield return new WaitForSeconds(0.15f);
        DetectAndAttackTarget();
    }
    private void PosTargetMin()
    {
        foreach (var col in enemyInsideArea)
        {
            _enemyPos.Add(col.GetComponent<Character>());
        }

        _distanceEnemy.Clear();
        foreach (var dist in _enemyPos.Select(dis => Vector3.Distance(dis.transform.position, transform.position)))
        {
            _distanceEnemy.Add(dist);
        }
        var minDis = _distanceEnemy.AsQueryable().Min();
        var indexInList = _distanceEnemy.IndexOf(minDis);
        
        enemyDetected = _enemyPos[indexInList];
        enemyPosMin = _enemyPos[indexInList].transform.position;
        if (!(Vector3.Distance(enemyDetected.transform.position, transform.position) <= AttackRange) ||
            _attackCountDown) return;
        Attack();
        _attackCountDown = true;
        DOVirtual.DelayedCall(AttackSpeed, () => _attackCountDown = false);
    }
}


[Serializable]
public class StatCharacter
{
    public StatType statType;
    public float baseValue;
    public float statIncrease;
    public float increaseTemporary;
    public float timeEffect;

    public float CurrentValue()
    {
        if (!(timeEffect > 0)) return baseValue + statIncrease;
        DOVirtual.DelayedCall(timeEffect, () => increaseTemporary = 0);
        return baseValue + statIncrease + baseValue * increaseTemporary;
    }

    public StatCharacter(StatType statType,  float baseValue = 0, float statIncrease = 0)
    {
        this.statType = statType;
        this.baseValue = baseValue;
        this.statIncrease = statIncrease;
    }
}

public enum StatType
{
    HP,
    SpeedVelocity,
    ATK,
    DetectRange,
    AttackSpeed,
    AttackRange
}
