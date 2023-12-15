using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : Character
{
    [SerializeField] private SpriteRenderer sprEnemy;

    private Camera _camera;
    private Vector3 _enemyMoveNotVisible;
    private Coroutine _insideCam;


    protected int currentHp;
    protected float speedVelocity;

    protected virtual void Start()
    {
        _camera = Camera.main;
        Init();
        CheckInSideCam();
    }
    
    protected override void Init()
    {
        base.Init();
        attackSpeed = stats.Find(s => s.statType == StatType.AttackSpeed).baseValue;
        attackRange = stats.Find(s => s.statType == StatType.AttackRange).baseValue;
        detectRange = stats.Find(s => s.statType == StatType.DetectRange).baseValue;
        speedVelocity = stats.Find(s => s.statType == StatType.SpeedVelocity).baseValue;
    }

    public void ShowEnemy(Sprite spr)
    {
        sprEnemy.sprite = spr;
        gameObject.Show();
    }

    private void Update()
    {
        EnemyMove();
        Flip(transform, targetPosMin);
    }

    private void Flip(Transform enemy, Vector2 target)
    {
        enemy.localScale = new Vector2(target.x > enemy.position.x ? 1 : -1, 1);
    }

    private void CheckInSideCam()
    {
        if (_insideCam != null) StopCoroutine(_insideCam);
        _insideCam = StartCoroutine(InsideCamera());
    }

    private void EnemyMove()
    {
        var positionTrans = transform.position;
        if (enemyInsideArea.Length <= 0)
        {
            positionTrans =
                Vector3.MoveTowards(positionTrans, _enemyMoveNotVisible.MapLimited(), speedVelocity * Time.deltaTime);
            transform.position = positionTrans;
        }
        else
        {
            transform.position = Vector3.Distance(positionTrans, targetPosMin) < attackRange - 0.2f
                ? positionTrans
                : Vector3.MoveTowards(transform.position, targetPosMin, speedVelocity * Time.deltaTime);
        }
    }

    protected virtual void Attack()
    {
    }

    private IEnumerator InsideCamera()
    {
        if (IsVisible(_camera, transform.position))
        {
            detectRange = 9.5f;
        }
        else
        {
            detectRange = 4.75f;
            var position = transform.position;
            var posX = Random.Range(position.x - 2, position.x + 2);
            var posY = Random.Range(position.y - 2, position.y + 2);
            _enemyMoveNotVisible = new Vector3(posX, posY, 0);
        }

        yield return new WaitForSeconds(1f);
        CheckInSideCam();
    }

    private bool IsVisible(Camera c, Vector3 target)
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
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Weapon"))
        {
            Destroy(gameObject);
        }
    }
}