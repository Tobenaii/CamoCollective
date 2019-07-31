using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectOpener : MonoBehaviour
{
    [SerializeField]
    private Vector3 m_openPos;
    [SerializeField]
    private Vector3 m_openScale;
    [SerializeField]
    private Vector3 m_closePos;
    [SerializeField]
    private Vector3 m_closeScale;
    [SerializeField]
    private float m_moveSpeed;

    private Vector3 m_velocity;
    private Vector3 m_scaleVelocity;
    private bool m_isOpen;
    private RectTransform m_rect;

    private void Start()
    {
        m_rect = GetComponent<RectTransform>();
        m_isOpen = true;
    }

    private void Update()
    {
        if (m_isOpen)
        {
            m_rect.anchoredPosition = Vector3.SmoothDamp(m_rect.anchoredPosition, m_openPos, ref m_velocity, m_moveSpeed * Time.deltaTime);
            m_rect.localScale = Vector3.SmoothDamp(m_rect.localScale, m_openScale, ref m_scaleVelocity, m_moveSpeed * Time.deltaTime);
        }
        else
        {
            m_rect.anchoredPosition = Vector3.SmoothDamp(m_rect.anchoredPosition, m_closePos, ref m_velocity, m_moveSpeed * Time.deltaTime);
            m_rect.localScale = Vector3.SmoothDamp(m_rect.localScale, m_closeScale, ref m_scaleVelocity, m_moveSpeed * Time.deltaTime);
        }
    }

    public void OpenCharacterSelect()
    {
        m_isOpen = true;
    }

    public void CloseCharacterSelect()
    {
        m_isOpen = false;
    }
}
