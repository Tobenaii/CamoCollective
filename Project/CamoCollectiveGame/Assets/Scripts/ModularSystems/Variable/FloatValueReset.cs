using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatValueReset : MonoBehaviour
{
    [SerializeField]
    private bool m_triggerOnAwake;
    [SerializeField]
    private List<FloatValue> m_floatValues;

    private void Awake()
    {
        if (m_triggerOnAwake)
            ResetValues();
    }

    public void ResetValues()
    {
        foreach (FloatValue value in m_floatValues)
            value.Reset();
    }
}
