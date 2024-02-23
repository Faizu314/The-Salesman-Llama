using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillTracker : MonoBehaviour
{
    [SerializeField] WetBarUI m_wetBarUI;
    [SerializeField] float m_maxValue;
    [SerializeField] AudioPlayer.PlayAtPointInput m_earnMoneySFX;

    public Action reachedMax;

    float currentValue = 0;

    public void AddValue(float value)
    {
        currentValue += value;
        if (currentValue >= m_maxValue)
        {
            currentValue = m_maxValue;
            if (GameManager.Instance.IsInGame)
            {
                reachedMax?.Invoke();
                AudioPlayer.PlayAtPoint(transform.position, m_earnMoneySFX);
            }
        }

        m_wetBarUI.UpdateFillBar(currentValue, m_maxValue);
    }

    public void ResetValue()
    {
        currentValue = 0;
    }
}
