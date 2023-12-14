using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 target;
    private float _speedBullet;

    public void InitBullet(int speed)
    {
        _speedBullet = speed;
    }

    private void Update()
    {
        Transform trans;
        (trans = transform).position = Vector3.MoveTowards(transform.position, target, _speedBullet * Time.deltaTime);
        trans.right = target - trans.position;
        if(trans.position == target) Destroy(gameObject);
    }
}