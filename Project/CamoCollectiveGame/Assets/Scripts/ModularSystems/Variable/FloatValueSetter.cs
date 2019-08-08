using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatValueSetter : MonoBehaviour
{
    [SerializeField]
    private FloatValue m_floatValue;

    public void SetValue(float value)
    {
        m_floatValue.Value = value;
    }
}
