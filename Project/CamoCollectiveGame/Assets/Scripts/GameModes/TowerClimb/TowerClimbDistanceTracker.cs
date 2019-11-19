using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerClimbDistanceTracker : MonoBehaviour
{
    [SerializeField]
    private FloatValue m_climbSpeed;
    [SerializeField]
    private float m_scale;
    [SerializeField]
    private TextMeshProUGUI m_textMeshPro;

    private float m_distance;
    private bool m_stopped;

    private void Update()
    {
        if (m_stopped)
            return;

        m_distance += m_climbSpeed.Value * Time.deltaTime * m_scale;
        m_textMeshPro.text = (int)m_distance + "m";
    }

    public void Stop()
    {
        m_stopped = true;
    }
}
