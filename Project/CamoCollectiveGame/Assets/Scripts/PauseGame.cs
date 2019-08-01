using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseGame : MonoBehaviour
{
    [SerializeField]
    private GameObject m_firstMenuItem;

    private void Awake()
    {
        EventSystem.current.SetSelectedGameObject(m_firstMenuItem);
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }
}
