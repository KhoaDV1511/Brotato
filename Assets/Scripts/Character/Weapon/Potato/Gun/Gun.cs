using DG.Tweening;
using UnityEngine;

public class Gun : Weapon
{
    [SerializeField] private Projectile projectile;
    [SerializeField] private GameObject effectBullet;

    private readonly PotatoModel _potatoModel = PotatoModel.Instance;
    
    private Sequence _attack;
    private Vector3 _direction;

    private void Start()
    {
        attackSpeed = 1;
        attackRange = 6;
        radius = 7;
        DetectAndAttackTarget();
    }

    private void Update()
    {
        _direction = _potatoModel.moveDirection == Vector3.zero
            ? Vector3.right
            : _potatoModel.moveDirection * 100;
        var target = enemyInsideArea.Length <= 0 ? _direction : targetPosMin;
        LookAtTarget(target, transform);
    }

    protected override void LookAtTarget(Vector3 target, Transform weaponPos)
    {
        base.LookAtTarget(target, weaponPos);
        
        Flip(target, weaponPos);
    }

    private void Flip(Vector3 target, Transform weaponPos)
    {
        transform.localScale = new Vector2 (1, target.x > weaponPos.position.x ? 1 : -1); 
    }

    protected override void Attack()
    {
        base.Attack();
        _attack?.Kill();
        var endValue = new Vector3(0, 0, AngleBetweenPoints(targetPosMin, transform.position));
        _attack = DOTween.Sequence().Append(transform.DORotate(endValue, 0.1f)).AppendCallback(() =>
        {
            var objBullet = Instantiate(projectile, transform);
            objBullet.target = targetPosMin;
            objBullet.Show();
            effectBullet.Show();
        }).AppendInterval(0.1f).AppendCallback(effectBullet.Hide);
    }

    protected override void DetectAndAttackTarget()
    {
        base.DetectAndAttackTarget();
        if(enemyInsideArea.Length > 0 && Vector3.Distance(targetPosMin, transform.position) <= attackRange)
            Attack();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}