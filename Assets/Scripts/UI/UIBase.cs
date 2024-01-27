using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIBase : MonoBehaviour
{
    CanvasGroup m_canvasGroup;

    private void Awake()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();
    }

    public virtual void TransitionIntoScreen()
    {
        m_canvasGroup.DOFade(1, 0.5f);
    }

    public virtual void TransitionOutofScreen()
    {
        m_canvasGroup.DOFade(0, 0.5f);
    }
}
