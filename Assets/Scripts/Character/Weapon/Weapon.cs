using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] LayerMask mask;
    public Collider2D[] enemyInsideArea;
    public Vector3 targetPosMin;
    private Coroutine _sweep;
    private readonly List<Vector3> _enemyPos = new List<Vector3>();
    private readonly List<float> _distanceEnemy = new List<float>();
    
    protected float attackSpeed;
    protected float attackRange;
    public float radius;
    protected virtual void LookAtTarget(Vector3 target, Transform weaponPos)
    {
        weaponPos.right = target - weaponPos.position;
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
        enemyInsideArea = Physics2D.OverlapCircleAll(transform.position, radius, mask);
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