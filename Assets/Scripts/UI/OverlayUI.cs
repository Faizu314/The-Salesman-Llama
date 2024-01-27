using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class OverlayUI : MonoBehaviour
{
    public static OverlayUI Instance;
    [SerializeField] TextMeshProUGUI m_moneyText;
    int currentMoney = 0;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void AddMoney(int add)
    {
        currentMoney += add;
        m_moneyText.text = currentMoney.ToString();
    }

    public void ResetMoney()
    {
        currentMoney = 0;
        m_moneyText.text = "0";
    }
}
