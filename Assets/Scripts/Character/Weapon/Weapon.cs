using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Weapon : Character
{
    private float RotationSpeed => enemyInsideArea.Length <= 0 ? 20 : 50;
    protected virtual void LookAtTarget(Vector3 target, Transform weaponPos)
    {
        var angle = AngleBetweenPoints(target, weaponPos.position);
        var targetRotation = Quaternion.Euler(new Vector3(0f,0f,enemyInsideArea.Length <= 0 ? -angle : angle));
        transform.rotation = Quaternion.Slerp(weaponPos.rotation, targetRotation, Time.deltaTime * RotationSpeed);
        
        float AngleBetweenPoints(Vector2 a, Vector2 b)
        {
            return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
        }
    }
    protected virtual void Attack()
    {
        
    }
}