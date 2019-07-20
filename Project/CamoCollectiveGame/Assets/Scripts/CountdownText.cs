using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownText : MonoBehaviour
{
    [SerializeField]
    private Text m_text;
    [SerializeField]
    private Countdown m_countdown;
    [SerializeField]
    private float m_offset;
    [SerializeField]
    private bool m_toInt;

    private void Update()
    {
        string value = ((m_toInt) ? ((int)m_countdown.GetTime() + m_offset).ToString() : (m_countdown.GetTime() + m_offset).ToString());
        m_text.text = value;
    }
}
