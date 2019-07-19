using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    [SerializeField]
    private bool m_startOnAwake;
    [SerializeField]
    private float m_startTime;
    [SerializeField]
    private FloatValue m_timeValue;
    [SerializeField]
    private GameEvent m_countdownFinishedEvent;
    private bool m_countdownStarted = false;
    private float m_countdown;

    private void Awake()
    {
        m_countdown = m_startTime;
        if (m_startOnAwake)
            StartCountdown();
    }

    public float GetTime()
    {
        return m_countdown;
    }

    private void Update()
    {
        if (!m_countdownStarted)
            return;
        m_countdown -= Time.deltaTime;
        if (m_timeValue != null)
            m_timeValue.value = m_countdown;
        if (m_countdown <= 0)
        {
            StopCountdown();
            m_countdownFinishedEvent?.Invoke();
        }
    }

    public void StopCountdown()
    {
        m_countdownStarted = false;
    }

    public void StartCountdown()
    {
        m_countdownStarted = true;
    }
}
