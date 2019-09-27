using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionScroller : MonoBehaviour
{
    [SerializeField]
    private float m_spacing;
    [SerializeField]
    private FloatValue m_currentSelectionValue;
    [SerializeField]
    private List<GameObject> m_selections;
    private RectTransform m_rect;
    private float m_prevSelection = -1;
    private Vector2 m_firstPos;

    private void OnValidate()
    {
        UpdateTransforms();
    }

    private void UpdateTransforms()
    {
        m_firstPos = transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition;
        for (int i = 1; i < transform.childCount; i++)
            transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition = m_firstPos + (Vector2.right * (m_spacing * i));
    }

    private void Start()
    {
        m_currentSelectionValue.Value = (int)Mathf.Repeat(m_currentSelectionValue.Value, m_selections.Count);
        m_rect = GetComponent<RectTransform>();
        m_rect.anchoredPosition = new Vector3((m_currentSelectionValue.Value - 1) * -m_spacing, m_rect.anchoredPosition.y, 0);
        InfiniteScroll();
    }

    private void InfiniteScroll()
    {
        int leftIndex = (int)Mathf.Repeat(m_currentSelectionValue.Value - 1, m_selections.Count);
        m_selections[leftIndex].GetComponent<RectTransform>().anchoredPosition = m_firstPos + (Vector2.right * (m_spacing * (m_currentSelectionValue.Value - 1)));
        m_selections[leftIndex].GetComponent<SelectionButton>().SetIndex((int)m_currentSelectionValue.Value - 1);

        int rightIndex = (int)Mathf.Repeat(m_currentSelectionValue.Value + 1, m_selections.Count);
        m_selections[rightIndex].GetComponent<RectTransform>().anchoredPosition = m_firstPos + (Vector2.right * (m_spacing * (m_currentSelectionValue.Value + 1)));
        m_selections[rightIndex].GetComponent<SelectionButton>().SetIndex((int)m_currentSelectionValue.Value + 1);
    }

    // Update is called once per frame
    void Update()
    {

        if (EventSystem.current.currentSelectedGameObject == null)
        {
            int curIndex = (int)Mathf.Repeat(m_currentSelectionValue.Value, m_selections.Count);
            EventSystem.current.SetSelectedGameObject(m_selections[curIndex]);
            UpdateTransforms();
        }

        Vector2 newPos = new Vector2((m_currentSelectionValue.Value - 1) * -m_spacing, m_rect.anchoredPosition.y);
        if (m_rect.anchoredPosition != newPos)
        {
            m_rect.anchoredPosition = Vector3.MoveTowards(m_rect.anchoredPosition, newPos, 300 * Time.deltaTime);
            (EventSystem.current.currentInputModule as CustomInputModule).BaseInput.SetHorizontal(false);
        }
        else
            (EventSystem.current.currentInputModule as CustomInputModule).BaseInput.SetHorizontal(true);
    }

    private void LateUpdate()
    {
        if (m_prevSelection != m_currentSelectionValue.Value)
        {
            InfiniteScroll();
        }
        m_prevSelection = m_currentSelectionValue.Value;
    }
}
