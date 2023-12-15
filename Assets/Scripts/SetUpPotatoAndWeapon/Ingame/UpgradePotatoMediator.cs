using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePotatoMediator : MonoBehaviour
{
    [SerializeField] private GameObject bgShopEquipment;
    [SerializeField] private Button btnStartNewWave;

    private EndWaveSignals _endWaveSignals = Signals.Get<EndWaveSignals>();

    private void Start()
    {
        btnStartNewWave.onClick.AddListener(StartNewWaveClick);
    }

    private void OnEnable()
    {
        _endWaveSignals.AddListener(StartUpdatePotato);
    }

    private void OnDisable()
    {
        _endWaveSignals.RemoveListener(StartUpdatePotato);
    }

    private void StartUpdatePotato()
    {
        Time.timeScale = 0;
        bgShopEquipment.Show();
    }

    private void StartNewWaveClick()
    {
        bgShopEquipment.Hide();
        Time.timeScale = 1;
    }
}