using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatEventTrigger : MonoBehaviour
{
    [SerializeField]
    private FloatEvent m_floatEvent;

    public void TriggerEvent(float p)
    {
        m_floatEvent.Invoke(p);
    }
}
