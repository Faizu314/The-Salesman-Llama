using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public enum GameState { Countdown, InGame, GameAboutToOver, GameOver }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] GameSettings m_gameSettings;

    public GameSettings Settings => m_gameSettings;
    public bool IsInGame => m_isInGame;

    public Action gameStarted;
    public Action<bool> gameOvered;

    float m_timeLeft;
    bool m_isInGame;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void StartGame()
    {
        UIManager.Instance.TransitionToScreen(UIScreen.OverlayMenu);
        GameCountdown().Forget();
    }

    public void GameOver()
    {
        m_isInGame = false;

        if (OverlayUI.Instance.CurrentMoney >= m_gameSettings.MoneyGoal)
        {
            UIManager.Instance.TransitionToScreen(UIScreen.GameOverWinMenu);
            gameOvered?.Invoke(true);
        }
        else
        {
            UIManager.Instance.TransitionToScreen(UIScreen.GameOverFailMenu);
            gameOvered?.Invoke(false);
        }
    }

    private async UniTaskVoid GameCountdown()
    {
        float countdown = 3;
        while (countdown > 0)
        {
            int seconds = Mathf.CeilToInt(countdown);
            OverlayUI.Instance.UpdateTimer(0, seconds, GameState.Countdown);
            countdown -= Time.deltaTime;
            await UniTask.NextFrame(this.GetCancellationTokenOnDestroy());
        }

        m_isInGame = true;
        gameStarted?.Invoke();

        m_timeLeft = m_gameSettings.GameTime;
        while (m_timeLeft > 0)
        {
            int seconds = Mathf.CeilToInt(m_timeLeft) % 60;
            int minutes = Mathf.CeilToInt(m_timeLeft) / 60;

            OverlayUI.Instance.UpdateTimer(
                minutes,
                seconds,
                m_timeLeft < m_gameSettings.GameOverCountdownTime ? GameState.GameAboutToOver : GameState.InGame
                );

            m_timeLeft -= Time.deltaTime;
            await UniTask.NextFrame(this.GetCancellationTokenOnDestroy());
        }
        OverlayUI.Instance.UpdateTimer(0, 0, GameState.GameOver);
        GameOver();
    }
}
