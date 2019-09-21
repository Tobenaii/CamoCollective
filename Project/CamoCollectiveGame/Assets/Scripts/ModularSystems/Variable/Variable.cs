using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Variable<T> : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField]
    private T value;
    [SerializeField]
    private int amount = 1;

    [System.NonSerialized]
    private T[] m_runtimeValues;
    public T Value { get { return m_runtimeValues[0]; } set { m_runtimeValues[0] = value; } }
    public int Count { get { return amount; } private set { } }
    public T GetValue(int index)
    {
        return m_runtimeValues[index];
    }

    public void SetValue(int index, T value)
    {
        m_runtimeValues[index] = value;
    }

    public void OnAfterDeserialize()
    {
        m_runtimeValues = new T[amount];
        for (int i = 0; i < m_runtimeValues.Length; i++)
            m_runtimeValues[i] = value;
    }

    public void Reset(int index)
    {
        m_runtimeValues[index] = value;
    }

    public void Reset()
    {
        for (int i = 0; i < m_runtimeValues.Length; i++)
            m_runtimeValues[i] = value;
    }

    public void OnBeforeSerialize()
    {
    }
}
