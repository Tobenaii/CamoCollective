using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pages : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> m_pages = new List<GameObject>();
    private GameObject m_currentPage;
    private int m_currentPageIndex;
    private float prevTriggerPrev;
    private float prevTriggerNext;

    private void Start()
    {
        foreach (GameObject obj in m_pages)
            obj.SetActive(false);
        m_currentPageIndex = 0;
        ChangePage(m_currentPageIndex);
    }

    private void ChangePage(int index)
    {
        if (m_pages.Count == 0)
            return;

        if (index < 0)
            index = m_pages.Count - 1;
        if (index >= m_pages.Count)
            index = 0;

        m_currentPageIndex = index;

        m_currentPage?.SetActive(false);
        m_currentPage = m_pages[index];
        m_currentPage.SetActive(true);
    }

    public void NextPage(float trigger)
    {
        if (trigger > 0 && prevTriggerNext == 0)
            ChangePage(m_currentPageIndex + 1);
        prevTriggerNext = trigger;
    }

    public void PreviousPage(float trigger)
    {
        if (trigger > 0 && prevTriggerPrev == 0)
            ChangePage(m_currentPageIndex - 1);
        prevTriggerPrev = trigger;
    }
}
