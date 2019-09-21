using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualCursor : MonoBehaviour
{
    [SerializeField]
    private PlayerData m_playerData;
    [SerializeField]
    private Vector3Reference m_cursorPos;
    private RectTransform m_rectTransform;
    private Image m_image;

    private void Start()
    {
        m_rectTransform = GetComponent<RectTransform>();
        m_image = GetComponent<Image>();
        m_cursorPos.Value = RectTransformUtility.WorldToScreenPoint(null, m_rectTransform.position);
        m_image.color = m_playerData.IndicatorColour;

    }

    private void Update()
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_image.canvas.transform as RectTransform, m_cursorPos.Value, m_image.canvas.worldCamera, out pos);
        transform.position = m_image.canvas.transform.TransformPoint(pos);
    }
}
