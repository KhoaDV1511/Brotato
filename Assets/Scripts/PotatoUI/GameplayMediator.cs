using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayMediator : MonoBehaviour
{
    [SerializeField] private Button btnPause, btnContinueGame, btnReloadGame, btnRevival, btnStartNewGame;
    [SerializeField] private GameObject panelPauseGame, panelPotatoDeath, parentTxtDameCaused;
    [SerializeField] private TextMeshProUGUI txtDameEnemy;
    
    private readonly PotatoModel _potatoModel = PotatoModel.Instance;
    private readonly PotatoDeathSignals _potatoDeathSignals = Signals.Get<PotatoDeathSignals>();
    private readonly DameCausedSignals _dameCausedSignals = Signals.Get<DameCausedSignals>();

    private void Start()
    {
        btnPause.onClick.AddListener(PauseGameClick);
        btnContinueGame.onClick.AddListener(ContinueGameClick);
        btnReloadGame.onClick.AddListener(ReloadGameClick);
        btnRevival.onClick.AddListener(PotatoRevival);
        btnStartNewGame.onClick.AddListener(StartNewGame);
    }

    private void OnEnable()
    {
        _potatoDeathSignals.AddListener(OnPotatoDeath);
        _dameCausedSignals.AddListener(DameCaused);
    }

    private void OnDisable()
    {
        _potatoDeathSignals.RemoveListener(OnPotatoDeath);
        _dameCausedSignals.RemoveListener(DameCaused);
    }
    private void DameCaused(int dame, Vector2 posTxtAppear)
    {
        // var objTxt = Instantiate(txtDameEnemy, parentTxtDameCaused.transform);
        // objTxt.transform.position = posTxtAppear;
        // objTxt.SetText(dame.ToString());
        // objTxt.Show();
        //
        // DOVirtual.DelayedCall(0.3f, () => Destroy(objTxt.gameObject));
    }
    private void OnPotatoDeath()
    {
        Time.timeScale = 0;
        panelPotatoDeath.Show();
    }

    private void PotatoRevival()
    {
        Signals.Get<PotatoRevivalSignals>().Dispatch();
        panelPotatoDeath.Hide();
        Time.timeScale = 1;
    }

    private void StartNewGame()
    {
        var currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
        Time.timeScale = 1;
    }

    private void PauseGameClick()
    {
        Time.timeScale = 0;
        panelPauseGame.Show();
    }

    private void ContinueGameClick()
    {
        Time.timeScale = 1;
        panelPauseGame.Hide();
    }

    private void ReloadGameClick()
    {
        Time.timeScale = 1;
        var currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}