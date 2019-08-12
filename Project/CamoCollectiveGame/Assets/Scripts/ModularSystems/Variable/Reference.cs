
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Reference<TType, TValue> where TValue : Variable<TType>
{
    [HideInInspector]
    [SerializeField]
    private int m_index;

    public bool useConstant = true;
    [SerializeField]
    private TType m_constant;
    [SerializeField]
    private TValue m_variable = null;
    public TType Value
    {
        get
        {
            return (useConstant) ? m_constant : m_variable.GetValue(m_index);
        }
        set
        {
            m_variable.SetValue(m_index, value);
        }
    }

    public void Reset()
    {
        m_variable.Reset(m_index);
    }
}
