using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventTrigger : MonoBehaviour
{
    [SerializeField]
    private bool m_triggerOnAwake;
    [SerializeField]
    private GameEvent m_gameEvent;

    private void Awake()
    {
        if (m_triggerOnAwake)
            m_gameEvent.Invoke();
    }

    public void TriggerEvent()
    {
        m_gameEvent.Invoke();
    }
}
