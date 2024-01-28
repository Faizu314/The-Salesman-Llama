using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialAttackInicatorUI : MonoBehaviour
{
    [SerializeField] private Image m_indicatorImage;
    Camera m_camera;
    Transform m_transform;
    Transform indicatorContainer;

    // Start is called before the first frame update
    void Start()
    {
        m_camera = Camera.main;
        m_transform = transform;
        indicatorContainer = m_indicatorImage.transform.parent;
        indicatorContainer.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!indicatorContainer.gameObject.activeSelf) return;
        var screenPos = m_camera.WorldToScreenPoint(m_transform.position);
        indicatorContainer.position = screenPos;
    }

    public void EnableIcon(Sprite sprite)
    {
        m_indicatorImage.sprite = sprite;
        indicatorContainer.gameObject.SetActive(true);
    }

    public void DisableIcon()
    {
        indicatorContainer.gameObject.SetActive(false);
    }
}
