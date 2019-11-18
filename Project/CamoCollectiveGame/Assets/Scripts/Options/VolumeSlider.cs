using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField]
    private Text m_tag;

    public void OnVolumeChanged(float value)
    {
        m_tag.text = ((int)value).ToString();
    }

}
