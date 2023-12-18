using System.Collections.Generic;
using UnityEngine;

public class Weapon : Character
{
    [SerializeField] protected SpriteRenderer sprWeapon;
    [SerializeField] private SpriteOutline spriteOutline;
    private readonly PotatoModel _potatoModel = PotatoModel.Instance;
    private List<WeaponStat> _weaponStats => GlobalData.Ins.weaponAndItemStats.weaponStats;
    private WeaponStat _weaponStat => _weaponStats.Find(w => w.id == _potatoModel.weaponId);
    

    public void SetWeapon(Sprite weapon, Color color)
    {
        sprWeapon.sprite = weapon;
        spriteOutline.color = color;
        spriteOutline.UpdateOutlineSprite();
    }
    
    public void Init(Tire tire)
    {
        stats.Clear();
        stats.Add(new StatCharacter(StatType.ATK, _weaponStat.DameAttack(tire)));
        stats.Add(new StatCharacter(StatType.AttackSpeed, _weaponStat.AttackSpeed(tire)));
        stats.Add(new StatCharacter(StatType.AttackRange, _weaponStat.AttackRange(tire)));
        stats.Add(new StatCharacter(StatType.DetectRange, _weaponStat.DetectRange(tire)));
        //Debug.Log($"Stats weapon: {dame}, {attackSpeed}, {attackRange}, {detectRange}");
    }
    private float RotationSpeed => enemyInsideArea.Length <= 0 ? 15 : 20;
    protected void LookAtTargetAndFlip(Transform weaponPos)
    {
        var direction = _potatoModel.moveDirection == Vector3.zero
            ? Vector3.right
            : _potatoModel.moveDirection;
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