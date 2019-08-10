
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Reference<TType, TValue> where TValue : Variable<TType>
{
    public bool useConstant = true;
    [SerializeField]
    private TType m_constant;
    [SerializeField]
    private TValue m_variable = null;
    public TType Value
    {
        get
        {
            return (useConstant) ? m_constant : m_variable.Value;
        }
    }
}
