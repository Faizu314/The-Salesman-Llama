using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuUI : UIBase
{
    public void OnClickStartGame()
    {
        GameManager.Instance.StartGame();
    }
}
