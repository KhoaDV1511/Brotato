using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePotatoMediator : MonoBehaviour
{
    [SerializeField] private ShopEquipmentMediator shopEquipmentMediator;
    [SerializeField] private GameObject bgUpgradePotato, bgShopEquipment;
    [SerializeField] private TextMeshProUGUI txtDropHarvest;
    [SerializeField] private Button btnStartNewWave, btnRoll;

    private readonly PotatoModel _potatoModel = PotatoModel.Instance;
    private readonly EndWaveSignals _endWaveSignals = Signals.Get<EndWaveSignals>();
    private readonly RefreshDropPicked _refreshDropPicked = Signals.Get<RefreshDropPicked>();

    private int _wave;

    private void Start()
    {
        btnStartNewWave.onClick.AddListener(StartNewWaveClick);
        btnRoll.onClick.AddListener(RollClick);
    }

    private void OnEnable()
    {
        _endWaveSignals.AddListener(StartUpdatePotato);
        _refreshDropPicked.AddListener(SetTxtDropPicked);
    }

    private void OnDisable()
    {
        _endWaveSignals.RemoveListener(StartUpdatePotato);
        _refreshDropPicked.RemoveListener(SetTxtDropPicked);
    }

    private void RollClick()
    {
        shopEquipmentMediator.RollEquipment(_wave);
    }

    private void SetTxtDropPicked()
    {
        txtDropHarvest.SetText(_potatoModel.dropItemPicked.ToString());
    }

    private void StartUpdatePotato(int wave)
    {
        Time.timeScale = 0;
        SetTxtDropPicked();
        if(_potatoModel.potatoLevelUp > 0)
        {
            Debug.Log(_potatoModel.potatoLevelUp);
            bgUpgradePotato.Show();
            _potatoModel.potatoLevelUp--;
        }
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