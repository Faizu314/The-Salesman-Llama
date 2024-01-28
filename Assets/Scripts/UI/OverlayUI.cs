using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class OverlayUI : UIBase
{
    public static OverlayUI Instance;
    [SerializeField] TextMeshProUGUI m_moneyText;
    [SerializeField] TextMeshProUGUI m_countdownText;
    [SerializeField] TextMeshProUGUI m_startText;
    [SerializeField] TextMeshProUGUI m_goalText;
    int m_currentMoney = 0;

    public int CurrentMoney => m_currentMoney;

    public override void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        base.Awake();
        m_startText.alpha = 0;
    }

    private void Start()
    {
        GameManager.Instance.gameStarted += OnGameStarted;
    }

    public override void TransitionIntoScreen()
    {
        m_goalText.text = $"Earn {GameManager.Instance.Settings.MoneyGoal} today!";
        ResetMoney();
        base.TransitionIntoScreen();
    }

    public void AddMoney(int add)
    {
        m_currentMoney += add;
        m_moneyText.text = $"+${m_currentMoney}";
    }

    public void ResetMoney()
    {
        m_currentMoney = 0;
        m_moneyText.text = $"+${m_currentMoney}";
    }

    public void UpdateTimer(int minutes, int seconds, GameState gameState)
    {
        m_countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        switch (gameState)
        {
            case GameState.Countdown:
                m_countdownText.color = Color.yellow;
                break;
            case GameState.InGame:
                m_countdownText.color = Color.white;
                break;
            case GameState.GameAboutToOver:
                m_countdownText.color = Color.red;
                break;
            case GameState.GameOver:
                m_countdownText.color = Color.red;
                break;
        }
    }

    public void OnGameStarted()
    {
        m_startText.alpha = 1;
        m_startText.DOFade(0, 1).SetDelay(2);
    }
}
