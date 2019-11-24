using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QualityMode : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.Audio.AudioMixer m_audioMixer;
    [SerializeField]
    private UnityEngine.Audio.AudioMixer m_musicMixer;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Quality"))
            PlayerPrefs.SetFloat("Quality", 6);
        if (!PlayerPrefs.HasKey("Sound"))
            PlayerPrefs.SetFloat("Sound", 10);
        if (!PlayerPrefs.HasKey("Music"))
            PlayerPrefs.SetFloat("Music", 10);

        float music = Mathf.Lerp(0, 1, Mathf.InverseLerp(0, 10, PlayerPrefs.GetFloat("Music")));
        float sound = Mathf.Lerp(0, 1, Mathf.InverseLerp(0, 10, PlayerPrefs.GetFloat("Sound")));

        m_audioMixer.SetFloat("Volume", Mathf.Log10(music) * 20);
        m_musicMixer.SetFloat("Volume", Mathf.Log10(sound) * 20);
        QualitySettings.SetQualityLevel((int)PlayerPrefs.GetFloat("Quality"));
        
    }
}
