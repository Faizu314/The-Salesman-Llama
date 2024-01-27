using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverWinMenuUI : UIBase
{
    [SerializeField] TextMeshProUGUI m_resultText;

    public override void TransitionIntoScreen()
    {
        m_resultText.text = $"Congrats! You earned ${OverlayUI.Instance.CurrentMoney} today!";
        base.TransitionIntoScreen();
    }
    public void Exit()
    {
        UIManager.Instance.TransitionToScreen(UIScreen.StartMenu);
    }
}
