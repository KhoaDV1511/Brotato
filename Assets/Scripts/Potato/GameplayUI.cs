using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtWaveAndTime;
    
    private readonly WaveTimeSignals _waveTimeSignals = Signals.Get<WaveTimeSignals>();
    private Coroutine _countDownTimerCoroutine;
    private WaitForSeconds _waitFor1Sec;
    private long _timeLeft = -100;
    private int _wave = -100;

    private void Start()
    {
        _waitFor1Sec = new WaitForSeconds(1);
    }
    private void OnEnable()
    {
        _waveTimeSignals.AddListener(SetWaveAndTime);
    }

    private void OnDisable()
    {
        _waveTimeSignals.RemoveListener(SetWaveAndTime);
    }

    private void SetWaveAndTime(TimePerWave timePerWave)
    {
        _timeLeft = timePerWave.time;
        _wave = timePerWave.wave;

        if(_countDownTimerCoroutine != null)
            StopCoroutine(_countDownTimerCoroutine);
        _countDownTimerCoroutine = StartCoroutine(StartCountDownTimer());
    }
    
    private void ShowTimeRemain()
    {
        txtWaveAndTime.SetText($"Wave: {_wave} - Time remain: {_timeLeft}");

    }

    private IEnumerator StartCountDownTimer()
    {
        ShowTimeRemain();
        while (isActiveAndEnabled)
        {
            yield return _waitFor1Sec;
            if (_timeLeft >= -1)
            {
                _timeLeft -= 1;
                ShowTimeRemain();
            }
        }
    }
}