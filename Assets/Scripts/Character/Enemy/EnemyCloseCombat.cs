using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyCloseCombat : Character
{
    [SerializeField] private CloseCombat closeCombat;
    private Camera _camera;
    private Vector3 _enemyMoveNotVisible;

    private Coroutine _insideCam;

    private void Start()
    {
        _camera = Camera.main;
        speed = 2;
        
        CheckInSideCam();
    }

    private void Update()
    {
        if (closeCombat.enemyInsideArea.Length <= 0)
        {
            var position = transform.position;
            position = Vector3.MoveTowards(position, MoveWithLimited(_enemyMoveNotVisible), speed * Time.deltaTime);
            transform.position = position;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, closeCombat.targetPosMin, speed * Time.deltaTime);
        }
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
            closeCombat.radius = 9.5f;
        }
        else
        {
            closeCombat.radius = 4.75f;
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
        if (col.CompareTag($"Bullet"))
        {
            Destroy(gameObject);
        }
    }
}