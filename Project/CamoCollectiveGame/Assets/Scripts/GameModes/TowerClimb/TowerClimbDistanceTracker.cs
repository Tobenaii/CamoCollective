using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerClimbDistanceTracker : MonoBehaviour
{
    [SerializeField]
    private FloatValue m_climbSpeed;
    [SerializeField]
    private float m_scale;
    [SerializeField]
    private Text m_text;

    private float m_distance;
    private bool m_stopped;

    private void Update()
    {
        if (m_stopped)
            return;

        m_distance += m_climbSpeed.Value * Time.deltaTime * m_scale;
        m_text.text = (int)m_distance + "m";
    }

    public void Stop()
    {
        m_stopped = true;
    }
}
