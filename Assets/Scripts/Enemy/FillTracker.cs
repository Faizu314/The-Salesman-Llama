using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillTracker : MonoBehaviour
{
    [SerializeField] WetBarUI m_wetBarUI;
    [SerializeField] float m_maxValue;

    public Action reachedMax;

    float currentValue = 0;

    public void AddValue(float value)
    {
        currentValue += value;
        if (currentValue >= m_maxValue)
        {
            currentValue = m_maxValue;
            reachedMax?.Invoke();
        }

        m_wetBarUI.UpdateFillBar(currentValue, m_maxValue);
    }

    public void ResetValue()
    {
        currentValue = 0;
    }
}
