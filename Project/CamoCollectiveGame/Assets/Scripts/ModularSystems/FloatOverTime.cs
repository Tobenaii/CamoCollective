using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatOverTime : MonoBehaviour
{
    [SerializeField]
    private FloatValue m_float;
    [SerializeField]
    private float m_increaseRate;
    [SerializeField]
    private bool m_cap;
    [SerializeField]
    private float m_maxCap;

    private void Update()
    {
        m_float.value += m_increaseRate * Time.deltaTime;
        if (m_cap && m_float.value > m_maxCap)
            m_float.value = m_maxCap;
    }
}
