using UnityEngine;

public class BulletEnemyNormal : Projectile
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(PotatoTag.PLAYER))
        {
            Debug.Log($"Dame Attacker {dameCaused}");
            if (target)
            {
                target.ReceiveDamage(StatType.HP, dameCaused);
            }
            Destroy(gameObject);
        }
    }
}