using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitFloatValue : MonoBehaviour
{
    [SerializeField]
    private float m_initValue;
    [SerializeField]
    private FloatValue m_floatValue;

    private void Awake()
    {
        m_floatValue.value = m_initValue;
    }
}
