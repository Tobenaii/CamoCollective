using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatToText : MonoBehaviour
{
    [SerializeField]
    private FloatReference m_floatValue;
    [SerializeField]
    private Text m_text;

    private void Update()
    {
        m_text.text = m_floatValue.Value.ToString();
    }
}
