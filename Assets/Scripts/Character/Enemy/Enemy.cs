using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : Character
{
    [SerializeField] private SpriteRenderer sprEnemy;

    private List<EnemyStat> _enemyStats => GlobalData.Ins.enemyStats.enemyStats;
    private EnemyStat _enemyStat => _enemyStats.Find(e => e.enemyType == _enemyType);
    
    private Camera _camera;
    private Vector3 _enemyMoveNotVisible;
    private Coroutine _insideCam;
    private EnemyType _enemyType;
    private TypeDetectRangeEnemy _typeDetectRangeEnemy;
    
    private readonly UpgradeItemSignals _upgradeItemSignals = Signals.Get<UpgradeItemSignals>();
    protected override void OnEnable()
    {
        base.OnEnable();
        _upgradeItemSignals.AddListener(UpdateEnemy);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _upgradeItemSignals.RemoveListener(UpdateEnemy);
    }

    private void UpdateEnemy(EquipmentItemInfo equipmentItemInfo)
    {
        var statType = equipmentItemInfo.ItemStat.statItemIncreases.Find(s => s.increaseFor == IncreaseFor.Enemy);
        if (statType != null)
        {
            stats.Find(s => s.statType == statType.statType).statIncrease += statType.statIncrease;
            Debug.Log("update enemy");
        }
    }

    protected override void Start()
    {
        base.Start();
        _camera = Camera.main;
        CheckInSideCam();
    }

    public void Init(int level, EnemyType enemyType)
    {
        _enemyType = enemyType;
        stats.Add(new StatCharacter(StatType.ATK, _enemyStat.DameAttack(level)));
        stats.Add(new StatCharacter(StatType.HP, _enemyStat.Health(level)));
        stats.Add(new StatCharacter(StatType.AttackSpeed, _enemyStat.attackSpeed));
        stats.Add(new StatCharacter(StatType.AttackRange, _enemyStat.attackRange));
        stats.Add(new StatCharacter(StatType.DetectRange, _enemyStat.DetectRange(_typeDetectRangeEnemy)));
        stats.Add(new StatCharacter(StatType.SpeedVelocity, _enemyStat.speedVelocity));
        //Debug.Log($"Stats enemy: {DameAttack}, {AttackSpeed}, {AttackRange}, {DetectRange}, {CurrentHp}");
    }

    public override void ReceiveDamage(StatType statType, float statIncrease)
    {
        base.ReceiveDamage(statType, statIncrease);
        if (statType == StatType.HP)
        {
            Signals.Get<DameCausedSignals>().Dispatch(Mathf.CeilToInt(statIncrease), transform.position);
            sprEnemy.color = Color.red;
            DOVirtual.DelayedCall(0.1f, () => sprEnemy.color = Color.white);
        }
        if(CurrentHp <= 0)
        {
            Signals.Get<EnemyDeathSignals>().Dispatch(transform.position);
            Destroy(gameObject);
        }
    }

    public void ShowEnemy(EnemyAttribute enemyAttribute)
    {
        sprEnemy.sprite = enemyAttribute.avatar;
        gameObject.Show();
    }

    private void Update()
    {
        if(PotatoModel.isHarvestToStore) return;
        EnemyMove();
        if(enemyDetected)
            Flip(transform, enemyDetected.transform.position);
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
                Vector3.MoveTowards(positionTrans, _enemyMoveNotVisible.MapLimited(), SpeedVelocity * Time.deltaTime);
            transform.position = positionTrans;
        }
        else
        {
            transform.position = Vector3.Distance(positionTrans, enemyDetected.transform.position) < AttackRange - 0.2f
                ? positionTrans
                : Vector3.MoveTowards(transform.position, enemyDetected.transform.position, SpeedVelocity * Time.deltaTime);
        }
    }

    private IEnumerator InsideCamera()
    {
        if (IsVisible(_camera, transform.position))
        {
            _typeDetectRangeEnemy = TypeDetectRangeEnemy.InsideCam;
        }
        else
        {
            _typeDetectRangeEnemy = TypeDetectRangeEnemy.OutSideCam;
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
}