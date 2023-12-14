using UnityEngine;

public class BulletEnemyNormal : Projectile
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag($"Player"))
        {
            Destroy(gameObject);
        }
    }
}