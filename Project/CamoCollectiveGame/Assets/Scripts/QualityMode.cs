using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QualityMode : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.Audio.AudioMixer m_audioMixer;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Quality"))
            PlayerPrefs.SetFloat("Quality", 6);
        if (!PlayerPrefs.HasKey("Sound"))
            PlayerPrefs.SetFloat("Sound", 10);
        if (!PlayerPrefs.HasKey("Music"))
            PlayerPrefs.SetFloat("Music", 10);
    }
}
