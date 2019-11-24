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

    private void Start()
    {
        Time.timeScale = 0;
        m_gamePausedEvent?.Invoke();
    }

    private void OnDestroy()
    {
        Time.timeScale = 1;
        m_gameResumedEvent?.Invoke();
    }
}
