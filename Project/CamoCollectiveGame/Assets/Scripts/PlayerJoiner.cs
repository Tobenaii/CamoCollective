using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJoiner : MonoBehaviour
{
    [SerializeField]
    private List<PlayerData> m_players;
    [SerializeField]
    private FloatValue m_playerControllers;

    private PlayerData m_currentPlayerData;

    public void Disconnected()
    {
        m_currentPlayerData.IsPlaying = false;
    }

    public void Join(int controllerNum)
    {
        if (m_currentPlayerData != null && m_currentPlayerData.IsPlaying)
            return;

        int index = 0;
        foreach (PlayerData player in m_players)
        {
            if (!player.IsPlaying)
            {
                player.IsPlaying = true;
                m_currentPlayerData = player;
                m_playerControllers.SetValue(index, controllerNum);
                break;
            }
            index++;
        }
    }
}
