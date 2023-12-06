using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 target;
    private float _speed;

    private void Start()
    {
        _speed = 10;
    }

    private void Update()
    {
        Transform trans;
        (trans = transform).position = Vector3.MoveTowards(transform.position, target, _speed * Time.deltaTime);
        trans.right = target - trans.position;
        
        if(Vector3.Distance(trans.position, target) < 0.5f) Destroy(gameObject);
    }
}