using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class EnemyMediator : MonoBehaviour
{
    [SerializeField] private Transform[] enemyType;
    [SerializeField] private Transform posEnemyAppear;
    
    private readonly PotatoModel _potatoModel = PotatoModel.Instance;
    private readonly EnemyModel _enemyModel = EnemyModel.Instance;
    
    private readonly StartGameSignals _startGameSignals = Signals.Get<StartGameSignals>();
    
    private Coroutine _rtEnemyWave;
    private Vector2 _posAppear;

    private void OnEnable()
    {
        _startGameSignals.AddListener(StartWaveEnemy);
    }

    private void OnDisable()
    {
        _startGameSignals.RemoveListener(StartWaveEnemy);
    }

    private void StartWaveEnemy()
    {
        _rtEnemyWave = StartCoroutine(EnemyWave());
    }
    
    private IEnumerator EnemyWave()
    {
        foreach (var wave in _enemyModel.TimePerWaves)
        {
            Signals.Get<WaveTimeSignals>().Dispatch(wave);
            
            var index = 0;
            float time = 0;
            while (time <= wave.time)
            {
                var turn = index;

                int Turn()
                {
                    return turn >= _enemyModel.WaveEnemies.Count - 1 ? _enemyModel.WaveEnemies.Count - 1 : turn;
                }
                time += _enemyModel.TurnReload(Turn(), wave.wave - 1);
                for (var k = 0; k < _enemyModel.EnemyQuantity(Turn(), wave.wave - 1); k++)
                {
                    this.WaitTimeout(() =>
                    {
                        _posAppear = PositionAppear();
                        var objAppear = Instantiate(posEnemyAppear, transform);
                        objAppear.position = _posAppear;
                        objAppear.Show();
                    
                        var objEnemy = Instantiate(enemyType[(int)EnemyType.EnemyCloseCombat], transform);
                        objEnemy.position = _posAppear;

                        objAppear.GetComponent<SpriteRenderer>().DOFade(0.2f, PotatoKey.TIME_PER_LOOP)
                            .From(1).SetLoops(PotatoKey.LOOPS).OnComplete((() =>
                            {
                                objAppear.GetComponent<SpriteRenderer>().ChangeAlpha(0);
                                Destroy(objAppear.gameObject);
                            }));
                        this.WaitTimeout(() =>
                        {
                            if(objEnemy != null)
                                objEnemy.Show();
                        }, PotatoKey.TIME_PER_LOOP * PotatoKey.LOOPS);
                    }, time);
                }
                
                index++;
            }

            yield return new WaitForSeconds(wave.time);
        }
    }

    private Vector3 PositionAppear()
    {
        var direction = new Vector3(Random.Range(-1, 1f), Random.Range(-1, 1f)).normalized * Random.Range(PotatoKey.DISTANCE_MIN, PotatoKey.DISTANCE_MAX);
        direction = new Vector3(direction.x + _potatoModel.potatoPos.x, direction.y + _potatoModel.potatoPos.y);
        return  direction.MapLimited();
    }
}

public enum EnemyType
{
    EnemyCloseCombat,
    EnemyFarCombat
}