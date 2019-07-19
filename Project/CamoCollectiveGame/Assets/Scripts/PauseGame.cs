using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseGame : MonoBehaviour
{
    [SerializeField]
    private GameObject m_firstMenuItem;

    public void TogglePause()
    {
        if (Time.timeScale > 0)
        {
            EventSystem.current.SetSelectedGameObject(m_firstMenuItem);
            Time.timeScale = 0;
        }
        else
            Time.timeScale = 1;
    }
}
