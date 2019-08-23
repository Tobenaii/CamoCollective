using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseGame : MonoBehaviour
{
    [SerializeField]
    private GameEvent m_gamePausedEvent;
    [SerializeField]
    private GameEvent m_gameResumedEvent;

    private void Awake()
    {
        Time.timeScale = 0;
        m_gamePausedEvent?.Invoke();
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
        m_gameResumedEvent?.Invoke();
    }
}
