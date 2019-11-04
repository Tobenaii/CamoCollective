using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatToText : MonoBehaviour
{
    [SerializeField]
    private FloatReference m_floatValue;
    [SerializeField]
    private bool m_toInt;
    [SerializeField]
    private float m_offset;
    [SerializeField]
    private Text m_text;

    private void Update()
    {
        if (!m_toInt)
            m_text.text = (m_floatValue.Value + m_offset).ToString();
        else
            m_text.text = ((int)m_floatValue.Value + m_offset).ToString();
    }
}
