using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMixer : MonoBehaviour
{
    enum TransitionState {Idle, FadeOut, FadeIn}

    [SerializeField]
    private float m_transitionTime;
    private AudioSource m_source;
    private AudioSource m_nextSource;

    private TransitionState m_state;
    private TimeLerper m_lerper;
    private float m_initVolume;

    private void Start()
    {
        m_lerper = new TimeLerper();
    }

    private void Update()
    {
        if (m_state == TransitionState.Idle)
            return;
        if (m_state == TransitionState.FadeOut)
        {
            float volume = m_lerper.Lerp(m_initVolume, 0, m_transitionTime);
            m_source.volume = volume;
            if (volume <= 0)
            {
                m_lerper.Reset();
                m_state = TransitionState.FadeIn;
                m_source = m_nextSource;
                m_source.Play();
            }
        }

        else if (m_state == TransitionState.FadeIn)
        {
            float volume = m_lerper.Lerp(0, 1, m_transitionTime);
            m_source.volume = volume;
            if (volume >= 1)
            {
                m_state = TransitionState.Idle;
                m_lerper.Reset();
            }
        }
    }

    public void SetMusic(AudioSource audio)
    {
        if (m_source == audio)
            return;
        if (m_source == null)
        {
            m_source = audio;
            m_source.Play();
            m_source.volume = 0;
            m_state = TransitionState.FadeIn;
            return;
        }
        m_state = TransitionState.FadeOut;
        m_initVolume = m_source.volume;
        m_nextSource = audio;
    }
}
