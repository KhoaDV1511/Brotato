using System;
using UnityEngine;

public class BulletPotato : Projectile
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag($"Enemy"))
        {
            Destroy(gameObject);
        }
    }
}