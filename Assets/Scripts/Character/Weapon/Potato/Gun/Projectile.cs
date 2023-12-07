using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 target;
    private float _speed;

    private void Start()
    {
        _speed = 30;
    }

    private void Update()
    {
        Transform trans;
        (trans = transform).position = Vector3.MoveTowards(transform.position, target, _speed * Time.deltaTime);
        trans.right = target - trans.position;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag($"Enemy"))
        {
            Destroy(gameObject);
        }
    }
}