using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialAttackInicatorUI : MonoBehaviour
{
    [SerializeField] private Image m_indicatorImage;
    [SerializeField] private CanvasGroup m_indicatorContainer;
    Camera m_camera;
    Transform m_transform;

    // Start is called before the first frame update
    void Start()
    {
        m_camera = Camera.main;
        m_transform = transform;
        m_indicatorContainer.alpha = 0;
        m_indicatorContainer.interactable = false;
        m_indicatorContainer.blocksRaycasts = false;
    }

    private void Update()
    {
        if (!m_indicatorContainer.gameObject.activeSelf) return;
        var screenPos = m_camera.WorldToScreenPoint(m_transform.position);
        m_indicatorContainer.transform.position = screenPos;
    }

    public void EnableIcon(Sprite sprite)
    {
        m_indicatorImage.sprite = sprite;
        m_indicatorContainer.alpha = 1;
    }

    public void DisableIcon()
    {
        m_indicatorContainer.alpha = 0;
    }
}
