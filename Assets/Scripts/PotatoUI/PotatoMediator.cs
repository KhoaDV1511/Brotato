using System;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PotatoMediator : MonoBehaviour
{
    [SerializeField] private Slider sldHealth, sldExperience;
    [SerializeField] private TextMeshProUGUI txtLevel, txtSldHp, txtDropItemQuantity, txtStoreDropItemQuantity;
    
    private readonly PotatoModel _potatoModel = PotatoModel.Instance;

    private readonly UpdateDropItemPickedSignals _updateDropItemPickedSignals =
        Signals.Get<UpdateDropItemPickedSignals>(); 

    private readonly RefreshDropPicked _refreshDropPicked = Signals.Get<RefreshDropPicked>();
    public int maxHp;

    private void OnEnable()
    {
        _updateDropItemPickedSignals.AddListener(SetValueLevel);
        _refreshDropPicked.AddListener(SetTxtDrop);
    }

    private void OnDisable()
    {
        _updateDropItemPickedSignals.RemoveListener(SetValueLevel);
        _refreshDropPicked.RemoveListener(SetTxtDrop);
    }

    public void InitHealthAndLevel(int currentHp)
    {
        maxHp = currentHp;
        _potatoModel.dropItemPicked = 20;
        SetHp(currentHp);
        SetLevel();
        SetStatDropItem();
    }

    public void SetValueHp(int currentHp)
    {
        txtSldHp.SetText($"{currentHp}/{maxHp}");
        sldHealth.SetValueWithoutNotify(currentHp);
    }

    public void SetValueLevel()
    {
        txtLevel.SetText($"LV.{_potatoModel.levelPotato}");
        _potatoModel.experienceCurrentPotato += 1;
        sldExperience.SetValueWithoutNotify(_potatoModel.experienceCurrentPotato);
        
        if (_potatoModel.experienceCurrentPotato >= _potatoModel.experienceMaxPerLevel)
        {
            _potatoModel.experienceCurrentPotato -= _potatoModel.experienceMaxPerLevel;
            _potatoModel.levelPotato += 1;
            _potatoModel.potatoLevelUp += 1;
            SetLevel();
        }

        SetStatDropItem();
    }
    
    private void SetTxtDrop()
    {
        txtDropItemQuantity.SetText(_potatoModel.dropItemPicked.ToString());
        txtStoreDropItemQuantity.SetText(_potatoModel.dropItemInfos.Count.ToString());
    }

    private void SetStatDropItem()
    {
        SetTxtDrop();
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