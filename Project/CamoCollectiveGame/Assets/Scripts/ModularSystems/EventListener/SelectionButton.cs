using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionButton : MonoBehaviour
{
    [SerializeField]
    private GameObjectEvent m_hoverEvent;
    [SerializeField]
    private GameEvent m_clickEvent;
    [SerializeField]
    private GameObject m_targetGameObject;
    private bool m_invoked;

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject && !m_invoked)
        {
            m_invoked = true;
            m_hoverEvent.Invoke(m_targetGameObject);
        }
        else if (EventSystem.current.currentSelectedGameObject != gameObject)
            m_invoked = false;
    }

    public void OnClick()
    {
        m_clickEvent.Invoke();
    }
}
