using System;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotatoMediator : MonoBehaviour
{
    [SerializeField] private Slider sldHealth, sldExperience;
    [SerializeField] private TextMeshProUGUI txtLevel, txtSldHp, txtDropItemQuantity, txtStoreDropItemQuantity;
    
    private readonly PotatoModel _potatoModel = PotatoModel.Instance;

    private readonly UpdateDropItemPickedSignals _updateDropItemPickedSignals =
        Signals.Get<UpdateDropItemPickedSignals>();
    private int _maxHp;

    private void OnEnable()
    {
        _updateDropItemPickedSignals.AddListener(SetValueLevel);
    }

    private void OnDisable()
    {
        _updateDropItemPickedSignals.RemoveListener(SetValueLevel);
    }

    public void InitHealthAndLevel(int currentHp)
    {
        _maxHp = currentHp;
        SetHp(currentHp);
        SetLevel();
        SetStatDropItem();
    }

    public void SetValueHp(int currentHp)
    {
        txtSldHp.SetText($"{currentHp}/{_maxHp}");
        sldHealth.SetValueWithoutNotify(currentHp);
    }

    public void SetValueLevel()
    {
        txtLevel.SetText(_potatoModel.levelPotato.ToString());
        sldExperience.SetValueWithoutNotify(_potatoModel.experienceCurrentPotato);

        if (_potatoModel.experienceCurrentPotato >= _potatoModel.experienceMaxPerLevel)
        {
            _potatoModel.experienceCurrentPotato -= _potatoModel.experienceMaxPerLevel;
            _potatoModel.levelPotato += 1;
            SetLevel();
        }

        SetStatDropItem();
    }

    private void SetStatDropItem()
    {
        txtDropItemQuantity.SetText(_potatoModel.dropItemPicked.ToString());
        txtStoreDropItemQuantity.SetText(_potatoModel.dropItemStore.ToString());
        sldExperience.SetValueWithoutNotify(_potatoModel.experienceCurrentPotato);
    }

    private void SetLevel()
    {
        sldExperience.maxValue = _potatoModel.experienceMaxPerLevel;
        txtLevel.SetText($"LV.{_potatoModel.levelPotato}");
        sldExperience.SetValueWithoutNotify(_potatoModel.experienceCurrentPotato);
    }

    private void SetHp(int currentHp)
    {
        sldHealth.maxValue = currentHp;
        SetValueHp(currentHp);
    }
}