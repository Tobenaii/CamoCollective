using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionScroller : MonoBehaviour
{
    [SerializeField]
    private float m_spacing;
    [SerializeField]
    private FloatValue m_currentSelectionValue;
    private RectTransform m_rect;

    private void OnValidate()
    {
        Vector2 firstPos = transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition;
        for (int i = 1; i < transform.childCount; i++)
            transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition = firstPos + (Vector2.right * (m_spacing * i));
    }

    private void Start()
    {
        m_rect = GetComponent<RectTransform>();
        m_rect.anchoredPosition = new Vector3((m_currentSelectionValue.Value - 1) * -m_spacing, m_rect.anchoredPosition.y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        m_rect.anchoredPosition = Vector3.MoveTowards(m_rect.anchoredPosition, new Vector3((m_currentSelectionValue.Value - 1) * -m_spacing, m_rect.anchoredPosition.y, 0), 300 * Time.deltaTime);
    }
}
