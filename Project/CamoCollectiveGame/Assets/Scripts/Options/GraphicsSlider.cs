using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class GraphicsSlider : MonoBehaviour
{
    [SerializeField]
    private Slider m_slider;
    [SerializeField]
    private Text m_tagText;
    [SerializeField]
    private List<string> m_graphicLevelTags;


    private void Awake()
    {
        m_slider.value = PlayerPrefs.GetFloat("Quality", m_slider.value);
    }

    private void OnValidate()
    {
        while (m_graphicLevelTags.Count <= m_slider.maxValue)
            m_graphicLevelTags.Add("Default");
        while (m_graphicLevelTags.Count > m_slider.maxValue + 1)
            m_graphicLevelTags.RemoveAt(m_graphicLevelTags.Count - 1);
    }


    public void OnGraphicsChanged(float value)
    {
        m_tagText.text = m_graphicLevelTags[(int)value];
        PlayerPrefs.SetFloat("Quality", value);
        QualitySettings.SetQualityLevel((int)PlayerPrefs.GetFloat("Quality"));
    }
}
