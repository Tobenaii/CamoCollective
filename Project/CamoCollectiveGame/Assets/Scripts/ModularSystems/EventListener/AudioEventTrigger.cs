using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEventTrigger : MonoBehaviour
{
    [SerializeField]
    private bool m_triggerOnAwake;
    [SerializeField]
    private AudioClip m_audioClip;
    [SerializeField]
    private AudioEvent m_audioEvent;

    private void Awake()
    {
        if (m_triggerOnAwake)
            m_audioEvent.Invoke(m_audioClip);
    }

    public void TriggerEvent()
    {
        m_audioEvent.Invoke(m_audioClip);
    }

    public void TriggerEvent(AudioClip p)
    {
        m_audioEvent.Invoke(p);
    }
}