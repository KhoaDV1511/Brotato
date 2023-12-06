using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyCloseCombat : Character
{
    private Camera _camera;
    private Vector3 _enemyMoveNotVisible;

    private void Start()
    {
        _camera = Camera.main;
        radius = 9.5f;
        speed = 2;
        attackSpeed = 1;
        attackRange = 0.7f;

        DetectAndAttackTarget();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, targetPosMin) > radius)
        {
            var position = transform.position;
            position = Vector3.MoveTowards(position, _enemyMoveNotVisible, speed * Time.deltaTime);
            transform.position = position;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosMin, speed * Time.deltaTime);
        }
    }

    protected override void DetectAndAttackTarget()
    {
        base.DetectAndAttackTarget();
        Attack();
    }

    private void Attack()
    {
        if (IsVisible(_camera, targetPosMin))
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
    }
}