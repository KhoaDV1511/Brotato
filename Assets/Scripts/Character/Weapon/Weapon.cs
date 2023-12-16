using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Weapon : Character
{
    [SerializeField] protected SpriteRenderer sprWeapon;
    [SerializeField] private SpriteOutline spriteOutline;
    private readonly PotatoModel _potatoModel = PotatoModel.Instance;

    public void SetWeapon(Sprite weapon, Color color)
    {
        spriteOutline.color = color;
        sprWeapon.sprite = weapon;
    }
    private float RotationSpeed => enemyInsideArea.Length <= 0 ? 15 : 20;
    protected void LookAtTargetAndFlip(Transform weaponPos)
    {
        var direction = _potatoModel.moveDirection == Vector3.zero
            ? Vector3.right
            : _potatoModel.moveDirection;
        var target = enemyInsideArea.Length <= 0
            ? direction.FindVectorADistanceVecTorB(origin: transform.localPosition, 10)
            : targetPosMin;
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
    protected virtual void Attack()
    {
        
    }
}