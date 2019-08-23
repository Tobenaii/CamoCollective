using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatValueReset : MonoBehaviour
{
    [SerializeField]
    private List<FloatValue> m_floatValues;

    public void ResetValues()
    {
        foreach (FloatValue value in m_floatValues)
            value.Reset();
    }
}
