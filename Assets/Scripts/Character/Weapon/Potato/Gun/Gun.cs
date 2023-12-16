using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class Gun : Weapon
{
    [SerializeField] private Projectile projectile;
    [SerializeField] private GameObject effectBullet;

    private readonly PotatoModel _potatoModel = PotatoModel.Instance;

    private Sequence _attack;

    private void Start()
    {
        Init();
        DetectAndAttackTarget();
    }

    protected override void Init()
    {
        base.Init();
        attackSpeed = stats.Find(s => s.statType == StatType.AttackSpeed).baseValue;
        attackRange = stats.Find(s => s.statType == StatType.AttackRange).baseValue;
        detectRange = stats.Find(s => s.statType == StatType.DetectRange).baseValue;
    }

    private void Update()
    {
        LookAtTargetAndFlip(transform);
    }

    [Button]
    private void EffectShoot()
    {
        var c = new Vector2(0.308f, 0.037f);
        var p = new Vector2(0.208f, 0.037f);
    }

    protected override void Attack()
    {
        base.Attack();
        _attack?.Kill();
        var posXGun = sprWeapon.transform.localPosition.x;
        var endValue = new Vector3(0, 0, AngleBetweenPoints(targetPosMin, transform.position));
        _attack = DOTween.Sequence().Append(transform.DORotate(endValue, 0.1f)).AppendCallback(Shoot)
            .Append(sprWeapon.transform.DOLocalMoveX(posXGun - 0.1f, 0.05f))
            .Append(sprWeapon.transform.DOLocalMoveX(posXGun, 0.05f)).AppendCallback(effectBullet.Hide);
    }

    private void Shoot()
    {
        var objBullet = Instantiate(projectile, transform);
        objBullet.target = targetPosMin;
        objBullet.InitBullet(30);
        objBullet.Show();
        effectBullet.Show();
    }

    protected override void DetectAndAttackTarget()
    {
        base.DetectAndAttackTarget();
        if (enemyInsideArea.Length > 0 && Vector3.Distance(targetPosMin, transform.position) <= attackRange)
            Attack();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}