using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventTrigger : MonoBehaviour
{
    [SerializeField]
    private GameEvent m_gameEvent;

    public void TriggerEvent()
    {
        m_gameEvent.Invoke();
    }
}
