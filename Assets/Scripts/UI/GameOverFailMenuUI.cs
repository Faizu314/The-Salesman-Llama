using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverFailMenuUI : UIBase
{
    public void Exit()
    {
        UIManager.Instance.TransitionToScreen(UIScreen.StartMenu);
    }
}
