using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIScreen { StartMenu, OverlayMenu, GameOverWinMenu, GameOverFailMenu }

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] UIScreen m_startScreen;
    [SerializeField] UIBase[] m_screens;
    [SerializeField] AudioPlayer.PlayNonSpatializedInput m_buttonClickSFX;
    [SerializeField] AudioPlayer.PlayNonSpatializedInput m_buttonHoverSFX;

    UIBase currentScreen;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        foreach (var screen in m_screens)
            screen.TransitionOutofScreen();
        TransitionToScreen(m_startScreen);
    }

    public void TransitionToScreen(UIScreen to)
    {
        if (currentScreen != null)
        {
            if (to == currentScreen.MyScreen) return;
            currentScreen.TransitionOutofScreen();
        }

        foreach (var screen in m_screens)
        {
            if (screen.MyScreen == to)
            {
                currentScreen = screen;
                break;
            }
        }
        currentScreen.TransitionIntoScreen();
    }

    public void PlayButtonClickSFX()
    {
        AudioPlayer.PlayNonSpatialized(m_buttonClickSFX);
    }

    public void PlayButtonHoverSFX()
    {
        AudioPlayer.PlayNonSpatialized(m_buttonHoverSFX);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
