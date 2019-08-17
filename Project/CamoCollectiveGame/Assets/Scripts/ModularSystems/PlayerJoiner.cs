using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJoiner : MonoBehaviour
{
    [SerializeField]
    private List<PlayerData> m_players;
    [SerializeField]
    private FloatValue m_playerControllers;

    [SerializeField]
    private FloatReference m_currentPlayerIndex;

    public void Disconnected()
    {
        m_players[(int)m_currentPlayerIndex.Value].IsPlaying = false;
    }

    public void Join(int controllerNum)
    {
        if (m_currentPlayerIndex.Value != -1 && m_players[(int)m_currentPlayerIndex.Value].IsPlaying)
            return;

        int index = 0;
        foreach (PlayerData player in m_players)
        {
            if (!player.IsPlaying)
            {
                player.IsPlaying = true;
                m_currentPlayerIndex.Value = index;
                m_playerControllers.SetValue(index, controllerNum);
                break;
            }
            index++;
        }
    }
}
