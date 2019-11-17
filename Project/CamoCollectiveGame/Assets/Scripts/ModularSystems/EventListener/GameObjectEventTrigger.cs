using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectEventTrigger : MonoBehaviour
{
    [SerializeField]
    private bool m_triggerOnAwake;
    [SerializeField]
    private GameObjectEvent m_gameObjectEvent;
    [SerializeField]
    private GameObject m_gameObject;

    private void Awake()
    {
        if (m_triggerOnAwake)
            m_gameObjectEvent.Invoke(m_gameObject);
    }

    public void TriggerEvent()
    {
        m_gameObjectEvent.Invoke(m_gameObject);
    }
}
