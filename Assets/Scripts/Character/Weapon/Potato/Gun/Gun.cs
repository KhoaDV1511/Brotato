using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class Gun : Weapon
{
    [SerializeField] private Projectile projectile;
    [SerializeField] private GameObject effectBullet;

    private Sequence _attack;

    private void Update()
    {
        LookAtTargetAndFlip(transform);
    }

    protected override void Attack()
    {
        base.Attack();
        _attack?.Kill();
        var posXGun = sprWeapon.transform.localPosition.x;
        var endValue = new Vector3(0, 0, AngleBetweenPoints(enemyDetected.transform.position, transform.position));
        _attack = DOTween.Sequence().Append(transform.DORotate(endValue, 0.07f)).AppendCallback(Shoot)
            .Append(sprWeapon.transform.DOLocalMoveX(posXGun - 0.1f, 0.04f))
            .Append(sprWeapon.transform.DOLocalMoveX(posXGun, 0.04f)).AppendCallback(effectBullet.Hide);
    }

    private void Shoot()
    {
        var objBullet = Instantiate(projectile, transform.position, Quaternion.identity);
        var target = enemyPosMin;
        objBullet.InitBullet(20, target, enemyDetected, DameAttack);
        effectBullet.Show();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, DetectRange);
    }
}