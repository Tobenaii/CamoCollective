using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField]
    private Text m_tag;
    [SerializeField]
    private string m_playerPref;
    [SerializeField]
    private Slider m_slider;
    [SerializeField]
    private UnityEngine.Audio.AudioMixer m_audioMixer;

    private void Awake()
    {
        m_slider.value = PlayerPrefs.GetFloat(m_playerPref);
    }

    public void OnVolumeChanged(float value)
    {
        m_tag.text = ((int)value).ToString();
        PlayerPrefs.SetFloat(m_playerPref, value);
        float vol = Mathf.Lerp(0.0001f, 1, Mathf.InverseLerp(0, 10, value));
        m_audioMixer.SetFloat("Volume", Mathf.Log10(vol) * 20);
    }

}
