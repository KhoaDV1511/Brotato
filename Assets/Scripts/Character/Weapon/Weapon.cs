using System.Collections.Generic;
using UnityEngine;

public class Weapon : Character
{
    [SerializeField] protected SpriteRenderer sprWeapon;
    [SerializeField] private SpriteOutline spriteOutline;
    private List<WeaponStat> _weaponStats => GlobalData.Ins.weaponAndItemStats.weaponStats;
    private WeaponStat _weaponStat => _weaponStats.Find(w => w.id == PotatoModel.weaponId);

    private readonly UpgradeItemSignals _upgradeItemSignals = Signals.Get<UpgradeItemSignals>();
    protected override void OnEnable()
    {
        base.OnEnable();
        _upgradeItemSignals.AddListener(UpdateWeapon);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _upgradeItemSignals.RemoveListener(UpdateWeapon);
    }

    private void UpdateWeapon(EquipmentItemInfo equipmentItemInfo)
    {
        var statType = equipmentItemInfo.ItemStat.statItemIncreases.Find(s => s.increaseFor == IncreaseFor.Weapon);
        if (statType != null)
        {
            stats.Find(s => s.statType == statType.statType).statIncrease += statType.statIncrease;
            Debug.Log("update weapon");
        }
    }

    public void SetWeapon(Sprite weapon, Color color)
    {
        sprWeapon.sprite = weapon;
        spriteOutline.color = color;
        spriteOutline.UpdateOutlineSprite();
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    public void Init(Tire tire)
    {
        //stats.Clear();
        stats.Add(new StatCharacter(StatType.MeleeAndRangedDame));
        stats.Add(new StatCharacter(StatType.ATK, _weaponStat.DameAttack(tire, MeleeAndRanged)));
        stats.Add(new StatCharacter(StatType.AttackSpeed, _weaponStat.AttackSpeed(tire)));
        stats.Add(new StatCharacter(StatType.AttackRange, _weaponStat.AttackRange(tire)));
        stats.Add(new StatCharacter(StatType.DetectRange, _weaponStat.DetectRange(tire)));
        
        //Debug.Log($"Stats weapon: {dame}, {attackSpeed}, {attackRange}, {detectRange}");
        Debug.Log(stats.Count);
    }
    private float RotationSpeed => enemyInsideArea.Length <= 0 ? 15 : 20;
    protected void LookAtTargetAndFlip(Transform weaponPos)
    {
        var direction = PotatoModel.moveDirection == Vector3.zero
            ? Vector3.right
            : PotatoModel.moveDirection;
        var target = enemyInsideArea.Length <= 0
            ? direction.FindVectorADistanceVecTorB(origin: transform.localPosition, 10)
            : enemyPosMin;
        var weaponPosition = enemyInsideArea.Length <= 0 ? weaponPos.localPosition : weaponPos.position;
        
        LookAtTarget(target, weaponPosition);
        Flip(target, weaponPosition);
    }

    private void LookAtTarget(Vector3 target, Vector3 weaponPos)
    {
        var angle = AngleBetweenPoints(target, weaponPos);
        var targetRotation = Quaternion.Euler(new Vector3(0f,0f,angle));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * RotationSpeed);
    }
    
    private void Flip(Vector3 target, Vector3 weaponPos)
    {
        transform.localScale = new Vector2(1, target.x > weaponPos.x ? 1 : -1);
    }
    
    protected float AngleBetweenPoints(Vector2 a, Vector2 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
}