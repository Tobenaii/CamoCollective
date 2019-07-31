using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FirstSeletedObjectUI : MonoBehaviour
{
    [SerializeField]
    private GameObject m_firstSelectedObject;

    private void OnEnable()
    {
        StartCoroutine(HighlightButton());
    }

    IEnumerator HighlightButton()
    {
        while (EventSystem.current == null)
            yield return null;
        EventSystem.current.SetSelectedGameObject(null);
        yield return null;
        EventSystem.current.SetSelectedGameObject(m_firstSelectedObject);
    }
}
