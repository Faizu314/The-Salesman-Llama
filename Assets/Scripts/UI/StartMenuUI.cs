using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuUI : UIBase
{
    public void OnClickStartGame()
    {
        GameManager.Instance.StartGame();
    }
}
