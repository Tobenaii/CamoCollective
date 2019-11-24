using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceValueSetter : MonoBehaviour
{
    [SerializeField]
    private AudioSourceValue m_audioSourceValue;
    [SerializeField]
    private AudioSource m_audioSource;

    private void Awake()
    {
        m_audioSourceValue.Value = m_audioSource;
    }
}
