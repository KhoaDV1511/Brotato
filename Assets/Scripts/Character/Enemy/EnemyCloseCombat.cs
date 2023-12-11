using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyCloseCombat : Character
{
    private Camera _camera;
    private Vector3 _enemyMoveNotVisible;

    private Coroutine _insideCam;

    private void Start()
    {
        _camera = Camera.main;

        InitEnemy();
        DetectAndAttackTarget();
        CheckInSideCam();
    }

    private void Update()
    {
        var positionTrans = transform.position;
        if (enemyInsideArea.Length <= 0)
        {
            positionTrans = Vector3.MoveTowards(positionTrans, _enemyMoveNotVisible.MapLimited(), speed * Time.deltaTime);
            transform.position = positionTrans;
        }
        else
        {
            transform.position = Vector3.Distance(positionTrans, targetPosMin) < 0.5f ? positionTrans : Vector3.MoveTowards(transform.position, targetPosMin, speed * Time.deltaTime);
        }
    }

    private void InitEnemy()
    {
        speed = 2;
        radius = 9.5f;
        attackSpeed = 1;
        attackRange = 0.7f;
    }
    
    protected override void DetectAndAttackTarget()
    {
        base.DetectAndAttackTarget();
        if(enemyInsideArea.Length > 0 && Vector3.Distance(targetPosMin, transform.position) <= attackRange)
            Attack();
    }

    private void Attack()
    {
        
    }

    private void CheckInSideCam()
    {
        if(_insideCam != null) StopCoroutine(_insideCam);
        _insideCam = StartCoroutine(InsideCamera());
    }

    private IEnumerator InsideCamera()
    {
        if (IsVisible(_camera, transform.position))
        {
            radius = 9.5f;
        }
        else
        {
            radius = 4.75f;
            var position = transform.position;
            var posX = Random.Range(position.x - 2, position.x + 2);
            var posY = Random.Range(position.y - 2, position.y + 2);
            _enemyMoveNotVisible = new Vector3(posX, posY, 0);
        }

        bool IsVisible(Camera c, Vector3 target)
        {
            var plans = GeometryUtility.CalculateFrustumPlanes(c);
            var point = target;

            foreach (var plan in plans)
            {
                if (plan.GetDistanceToPoint(point) < 0)
                {
                    return false;
                }
            }

            return true;
        }

        yield return new WaitForSeconds(1f);
        CheckInSideCam();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Weapon"))
        {
            Destroy(gameObject);
        }
    }
}