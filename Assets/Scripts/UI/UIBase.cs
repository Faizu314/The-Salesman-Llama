using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class UIBase : MonoBehaviour
{
    [SerializeField] UIScreen m_myScreen;

    public UIScreen MyScreen => m_myScreen;

    CanvasGroup m_canvasGroup;

    public virtual void Awake()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();
    }

    public virtual void TransitionIntoScreen()
    {
        m_canvasGroup.DOFade(1, 0.5f);
        m_canvasGroup.interactable = true;
        m_canvasGroup.blocksRaycasts = true;
    }

    public virtual void TransitionOutofScreen()
    {
        m_canvasGroup.DOFade(0, 0.5f);
        m_canvasGroup.interactable = false;
        m_canvasGroup.blocksRaycasts = false;
    }
}
