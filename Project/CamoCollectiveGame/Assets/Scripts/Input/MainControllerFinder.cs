using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainControllerFinder : MonoBehaviour
{
    [SerializeField]
    private List<PlayerData> m_playerData;
    [SerializeField]
    private FloatValue m_controllers;
    [SerializeField]
    private FloatValue m_mainController;

    private void Update()
    {
        int i = 0;
        foreach (PlayerData player in m_playerData)
        {
            if (player.IsPlaying)
            {
                m_mainController.Value = m_controllers.GetValue(i);
                return;
            }
            i++;
        }
    }
}
