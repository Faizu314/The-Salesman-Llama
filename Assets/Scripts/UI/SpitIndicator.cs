using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class SpitIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_sample;
    Camera m_camera;
    Transform m_transform;
    ObjectPool<TextMeshProUGUI> m_pool;

    // Start is called before the first frame update
    void Start()
    {
        m_camera = Camera.main;
        m_transform = transform;
        m_sample.alpha = 0;
        m_pool = new ObjectPool<TextMeshProUGUI>(CreateSpit, GetSpit);
    }

    private TextMeshProUGUI CreateSpit()
    {
        var proj = Instantiate(m_sample.gameObject, m_sample.transform.parent).GetComponent<TextMeshProUGUI>();
        return proj;
    }

    private void GetSpit(TextMeshProUGUI proj)
    {
        var screenPos = m_camera.WorldToScreenPoint(m_transform.position);
        proj.transform.position = screenPos;
        proj.alpha = 1;
        proj.DOFade(0, 1f).SetDelay(0.5f).OnComplete(() =>
        {
            m_pool.Release(proj);
        });
        proj.transform.DOMoveY(screenPos.y + 10, 1.5f);
    }

    public void EnableIndicator()
    {
        if (m_pool != null)
            m_pool.Get();
    }
}
