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
    private float m_prev;
    private bool m_frozen;

    private void Update()
    {
        if (m_frozen)
            return;
        m_float.Value += m_increaseRate * Time.deltaTime;
        if (m_cap && m_float.Value > m_maxCap)
            m_float.Value = m_maxCap;
    }
}
