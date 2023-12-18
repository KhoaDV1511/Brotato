using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePotatoMediator : MonoBehaviour
{
    [SerializeField] private ShopEquipmentMediator shopEquipmentMediator;
    [SerializeField] private GameObject bgShopEquipment;
    [SerializeField] private Button btnStartNewWave, btnRoll;

    private readonly PotatoModel _potatoModel = PotatoModel.Instance;
    private readonly EndWaveSignals _endWaveSignals = Signals.Get<EndWaveSignals>();

    private int _wave;

    private void Start()
    {
        btnStartNewWave.onClick.AddListener(StartNewWaveClick);
        btnRoll.onClick.AddListener(RollClick);
    }

    private void OnEnable()
    {
        _endWaveSignals.AddListener(StartUpdatePotato);
    }

    private void OnDisable()
    {
        _endWaveSignals.RemoveListener(StartUpdatePotato);
    }

    private void RollClick()
    {
        shopEquipmentMediator.RollEquipment(_wave);
    }

    private void StartUpdatePotato(int wave)
    {
        Time.timeScale = 0;
        bgShopEquipment.Show();
        _wave = wave;
        shopEquipmentMediator.RollEquipment(wave);
    }

    private void StartNewWaveClick()
    {
        Signals.Get<StartNewWaveSignals>().Dispatch();
        bgShopEquipment.Hide();
        Time.timeScale = 1;
    }
}