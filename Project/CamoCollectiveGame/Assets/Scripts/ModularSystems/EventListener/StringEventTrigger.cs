using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringEventTrigger : MonoBehaviour
{
    [SerializeField]
    private StringEvent m_stringEvent;

    public void Invoke(string s)
    {
        m_stringEvent.Invoke(s);
    }
}
