using System;
using UnityEngine;

public class BulletPotato : Projectile
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(PotatoTag.ENEMY))
        {
            if(target)
            {
                target.ReceiveDamage(StatType.HP, dameCaused);
            }
            if(!Equals(col.GetComponent<Character>(), target))
            {
                col.GetComponent<Character>().ReceiveDamage(StatType.HP, dameCaused * 0.7f);
            }
            Destroy(gameObject);
        }
    }
}