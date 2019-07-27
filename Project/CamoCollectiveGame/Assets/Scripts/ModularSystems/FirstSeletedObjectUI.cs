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
        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(HighlightButton());
    }

    IEnumerator HighlightButton()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(m_firstSelectedObject);
    }
}
