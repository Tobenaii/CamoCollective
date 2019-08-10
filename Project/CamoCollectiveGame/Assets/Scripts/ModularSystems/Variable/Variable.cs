using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Variable<T> : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField]
    private T value;

    [System.NonSerialized]
    private T m_runtimeValue;
    public T Value { get { return m_runtimeValue; } set { m_runtimeValue = value; } }

    public void OnAfterDeserialize()
    {
        m_runtimeValue = value;
    }

    public void OnBeforeSerialize()
    {
    }
}
