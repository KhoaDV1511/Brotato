using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 _targetPos;
    protected Character target;
    protected float dameCaused;
    private float _speedBullet;

    private readonly EndWaveSignals _endWaveSignals = Signals.Get<EndWaveSignals>();
    private readonly PotatoRevivalSignals _potatoRevivalSignals = Signals.Get<PotatoRevivalSignals>();

    private void OnEnable()
    {
        _endWaveSignals.AddListener(DesTroyBullet);
        _potatoRevivalSignals.AddListener(DesTroyBullet);
    }

    private void OnDisable()
    {
        _endWaveSignals.RemoveListener(DesTroyBullet);
        _potatoRevivalSignals.RemoveListener(DesTroyBullet);
    }

    private void DesTroyBullet(int obj)
    {
        Destroy(gameObject);
    }
    
    private void DesTroyBullet()
    {
        Destroy(gameObject);
    }

    public void InitBullet(int speed, Vector3 targetPos, Character targetAttack, float dameToTarget)
    {
        _targetPos = targetPos;
        _speedBullet = speed;
        target = targetAttack;
        dameCaused = dameToTarget;
        gameObject.Show();
    }

    private void Update()
    {
        Transform trans;
        (trans = transform).position = Vector3.MoveTowards(transform.position, _targetPos, _speedBullet * Time.deltaTime);
        trans.right = _targetPos - trans.position;
        if(Vector3.Distance(transform.position, _targetPos) <= 0.2f) Destroy(gameObject);
    }
}