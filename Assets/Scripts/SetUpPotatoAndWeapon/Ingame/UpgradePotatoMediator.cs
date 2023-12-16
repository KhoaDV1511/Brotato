using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePotatoMediator : MonoBehaviour
{
    [SerializeField] private ShopEquipmentMediator shopEquipmentMediator;
    [SerializeField] private UpgradeWeaponMediator upgradeWeaponMediator;
    [SerializeField] private GameObject bgShopEquipment;
    [SerializeField] private Button btnStartNewWave;

    private readonly PotatoModel _potatoModel = PotatoModel.Instance;
    private readonly EndWaveSignals _endWaveSignals = Signals.Get<EndWaveSignals>();

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

    private void StartUpdatePotato(int wave)
    {
        Time.timeScale = 0;
        bgShopEquipment.Show();
        shopEquipmentMediator.RollEquipment(wave);
    }

    private void StartNewWaveClick()
    {
        _potatoModel.currentWeaponValue = upgradeWeaponMediator.elementWeaponUpgrades.Count;
        Signals.Get<StartNewWaveSignals>().Dispatch(upgradeWeaponMediator.elementWeaponUpgrades);
        bgShopEquipment.Hide();
        Time.timeScale = 1;
    }
}