using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolValueSetter : MonoBehaviour
{
    [SerializeField]
    private BoolValue m_boolValue;
    [SerializeField]
    private bool m_onAwake;
    [SerializeField]
    private bool m_onUpdate;
    [SerializeField]
    private bool m_initValue;


    private void Awake()
    {
        if (m_onAwake)
            SetValue(m_initValue);
    }

    private void Update()
    {
        SetValue(m_initValue);
    }

    public void SetValue(bool value)
    {
        m_boolValue.Value = value;
    }
}
