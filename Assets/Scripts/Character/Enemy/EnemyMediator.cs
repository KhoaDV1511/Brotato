using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using static System.Random;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class EnemyMediator : MonoBehaviour
{
    [SerializeField] private Enemy[] enemyType;
    [SerializeField] private Transform posEnemyAppear;
    
    private readonly PotatoModel _potatoModel = PotatoModel.Instance;
    private readonly EnemyModel _enemyModel = EnemyModel.Instance;
    
    private readonly StartGameSignals _startGameSignals = Signals.Get<StartGameSignals>();
    private readonly PotatoRevivalSignals _potatoRevivalSignals = Signals.Get<PotatoRevivalSignals>();
    
    private Coroutine _rtEnemyWave;
    private Vector2 _posAppear;

    private void OnEnable()
    {
        _startGameSignals.AddListener(StartWaveEnemy);
        _potatoRevivalSignals.AddListener(DestroyEnemy);
    }

    private void OnDisable()
    {
        _startGameSignals.RemoveListener(StartWaveEnemy);
        _potatoRevivalSignals.RemoveListener(DestroyEnemy);
    }

    private void StartWaveEnemy()
    {
        if(_rtEnemyWave != null) StopCoroutine(_rtEnemyWave);
        _rtEnemyWave = StartCoroutine(EnemyWave());
    }

    private void DestroyEnemy()
    {
        transform.DestroyChildren();
    }
    
    private IEnumerator EnemyWave()
    {
        foreach (var wave in _enemyModel.TimePerWaves)
        {
            DestroyEnemy();
            Signals.Get<WaveTimeSignals>().Dispatch(wave);
            var index = 0;
            float time = 0;
            while (time <= wave.time)
            {
                var turn = index;
                time += _enemyModel.TurnReload(TurnInWave(turn), wave.wave - 1);
                for (var k = 0; k < _enemyModel.EnemyQuantity(TurnInWave(turn), wave.wave - 1); k++)
                {
                    EnemyPerTurn(time, TurnInWave(turn), wave.wave);
                }
                index++;
            }
            yield return new WaitForSeconds(wave.time);
            if (wave.wave == _enemyModel.TimePerWaves[^1].wave) yield break;
            Signals.Get<EndWaveSignals>().Dispatch(wave.wave);
        }
    }

    private void EnemyPerTurn(float time, int turn, int wave)
    {
        this.WaitTimeout(() =>
        {
            _posAppear = PositionAppear();
            WarningEnemyAppear(_posAppear);
            EnemyAppear(_posAppear, turn, wave);
        }, time);
    }
    private void EnemyAppear(Vector2 posAppear, int turn, int wave)
    {
        var enemyAttribute = _enemyModel.WaveEnemies[turn].enemyAttribute;
        var objEnemy = Instantiate(enemyType[(int)enemyAttribute.enemyType], transform);
        objEnemy.transform.position = posAppear;
        objEnemy.Init(enemyAttribute.LevelEnemyPerWave(wave, turn));
        
        this.WaitTimeout(() =>
        {
            if(objEnemy != null)
                objEnemy.ShowEnemy(enemyAttribute);
        }, PotatoKey.TIME_PER_LOOP * PotatoKey.LOOPS);
    }
    private void WarningEnemyAppear(Vector2 posAppear)
    {
        var objAppear = Instantiate(posEnemyAppear, transform);
        objAppear.position = posAppear;
        objAppear.Show();
        
        objAppear.GetComponent<SpriteRenderer>().DOFade(0.2f, PotatoKey.TIME_PER_LOOP)
            .From(1).SetLoops(PotatoKey.LOOPS).OnComplete((() =>
            {
                objAppear.GetComponent<SpriteRenderer>().ChangeAlpha(0);
                Destroy(objAppear.gameObject);
            }));
    }
    private int TurnInWave(int turn)
    {
        return turn >= _enemyModel.WaveEnemies.Count - 1 ? _enemyModel.WaveEnemies.Count - 1 : turn;
    }

    private Vector3 PositionAppear()
    {
        var direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * Random.Range(PotatoKey.DISTANCE_MIN, PotatoKey.DISTANCE_MAX);
        direction = new Vector3(direction.x + _potatoModel.potatoPos.x, direction.y + _potatoModel.potatoPos.y);
        return  direction.MapLimited();
    }
}