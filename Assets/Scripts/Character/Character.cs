using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] LayerMask mask;
    public Collider2D[] enemyInsideArea;
    public Vector3 targetPosMin;
    private Coroutine _sweep;
    private readonly List<Vector3> _enemyPos = new List<Vector3>();
    private readonly List<float> _distanceEnemy = new List<float>();
    
    protected float detectRange;
    protected float attackSpeed;
    protected float attackRange;
    [SerializeField] protected List<StatCharacter> stats = new List<StatCharacter>();
    
    protected virtual void Init()
    {
        
    }
    protected virtual void DetectAndAttackTarget()
    {
        if(_sweep != null) StopCoroutine(_sweep);
        _sweep = StartCoroutine(TargetPos());
    }
    private IEnumerator TargetPos()
    {
        _enemyPos.Clear();
        // ReSharper disable once Unity.PreferNonAllocApi
        enemyInsideArea = Physics2D.OverlapCircleAll(transform.position, detectRange, mask);
        if(enemyInsideArea.Length > 0)
            PosTargetMin();
 
        yield return new WaitForSeconds(attackSpeed);
        DetectAndAttackTarget();
    }
    private void PosTargetMin()
    {
        foreach (var col in enemyInsideArea)
        {
            _enemyPos.Add(col.transform.position);
        }

        _distanceEnemy.Clear();
        foreach (var dist in _enemyPos.Select(dis => Vector3.Distance(dis, transform.position)))
        {
            _distanceEnemy.Add(dist);
        }
        var minDis = _distanceEnemy.AsQueryable().Min();
        var indexInList = _distanceEnemy.IndexOf(minDis);
        
        targetPosMin = _enemyPos[indexInList];
    }
}


[Serializable]
public class StatCharacter
{
    public StatType statType;
    public float baseValue;
    public float statIncrease;
    public float timeEffect;

    public float CurrentValue()
    {
        return baseValue + baseValue * statIncrease;
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
