using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEventTrigger : MonoBehaviour
{
    [SerializeField]
    private bool m_triggerOnAwake;
    [SerializeField]
    private AudioSourceReference m_audioSource;
    [SerializeField]
    private AudioEvent m_audioEvent;

    private void Awake()
    {
        if (m_triggerOnAwake)
            m_audioEvent.Invoke(m_audioSource.Value);
    }

    private void OnEnable()
    {
        if (m_triggerOnAwake)
            m_audioEvent.Invoke(m_audioSource.Value);
    }

    public void TriggerEvent()
    {
        m_audioEvent.Invoke(m_audioSource.Value);
    }

    public void TriggerEvent(AudioSource p)
    {
        m_audioEvent.Invoke(p);
    }
}