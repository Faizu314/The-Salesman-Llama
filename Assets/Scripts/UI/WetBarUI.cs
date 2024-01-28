using UnityEngine;

public class WetBarUI : MonoBehaviour
{
    [SerializeField] Transform m_fillContainer;
    [SerializeField] RectTransform m_fill;

    public RectTransform FillBar => m_fill;

    Camera m_camera;
    float m_offsetX;
    Vector2 m_maxFill;

    private void Awake()
    {
        m_camera = Camera.main;
        m_offsetX = 20f / 1920f * Screen.width;
        m_maxFill = m_fill.sizeDelta;
    }

    private void OnEnable()
    {
        m_fillContainer.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!m_fillContainer.gameObject.activeSelf) return;
        var screenPos = m_camera.WorldToScreenPoint(transform.position);
        screenPos.x = screenPos.x + m_offsetX;
        m_fillContainer.transform.position = screenPos;
    }

    public void UpdateFillBar(float value, float maxValue)
    {
        float fill = value / maxValue;
        m_fill.sizeDelta = new Vector2(m_maxFill.x, fill * m_maxFill.y);
        if (!m_fillContainer.gameObject.activeSelf)
            m_fillContainer.gameObject.SetActive(true);
    }
}
