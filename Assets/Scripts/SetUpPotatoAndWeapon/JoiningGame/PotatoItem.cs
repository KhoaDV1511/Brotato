using System;
using EnhancedUI.EnhancedScroller;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotatoItem : EnhancedScrollerCellView
{
    [SerializeField] private Image imgPotato;
    [SerializeField] private Button btnPotato;

    private readonly PotatoModel _potatoModel = PotatoModel.Instance;

    private int _id;

    private void Start()
    {
        btnPotato.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        _potatoModel.potatoId = _id;
    }

    public void SetData(PotatoInfo potato)
    {
        imgPotato.sprite = potato.GetSprite();
        _id = potato.id;
    }
}