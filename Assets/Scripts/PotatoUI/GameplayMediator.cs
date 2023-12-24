using System;
using Sirenix.OdinInspector.Editor.Drawers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayMediator : MonoBehaviour
{
    [SerializeField] private Button btnPause, btnContinueGame, btnReloadGame, btnRevival, btnStartNewGame;
    [SerializeField] private GameObject panelPauseGame, panelPotatoDeath;
    [SerializeField] private TextMeshProUGUI txtDameEnemy;
    
    private readonly PotatoModel _potatoModel = PotatoModel.Instance;
    private readonly PotatoDeathSignals _potatoDeathSignals = Signals.Get<PotatoDeathSignals>();

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
    }

    private void OnDisable()
    {
        _potatoDeathSignals.RemoveListener(OnPotatoDeath);
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