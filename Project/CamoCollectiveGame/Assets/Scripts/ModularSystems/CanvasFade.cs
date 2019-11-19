using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasFade : MonoBehaviour
{
    [SerializeField]
    private float m_fadeInSpeed;

    private CanvasGroup m_canvasGroup;
    private float m_opacity;

    private void OnEnable()
    {
        m_opacity = 0;
        m_canvasGroup.alpha = m_opacity;
    }

    private void Start()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        m_canvasGroup.alpha = m_opacity;
        m_opacity += m_fadeInSpeed * Time.deltaTime;
        m_opacity = Mathf.Clamp(m_opacity, 0, 1);
    }
}
