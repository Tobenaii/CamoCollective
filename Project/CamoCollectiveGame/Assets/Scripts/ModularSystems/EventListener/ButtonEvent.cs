using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEvent : MonoBehaviour
{
    [SerializeField]
    private GameObjectEvent m_event;
    [SerializeField]
    private GameObject m_targetGameObject;
    private bool m_invoked;

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject && !m_invoked)
        {
            m_invoked = true;
            m_event.Invoke(m_targetGameObject);
        }
        else if (EventSystem.current.currentSelectedGameObject != gameObject)
            m_invoked = false;
    }
}
