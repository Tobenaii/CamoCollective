using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJoiner : MonoBehaviour
{
    [SerializeField]
    private List<PlayerData> m_players;
    [SerializeField]
    private List<CharacterData> m_characters;
    [SerializeField]
    private FloatValue m_playerControllers;
    [SerializeField]
    private BoolValue m_characterSelectOpen;
    [SerializeField]
    private FloatReference m_currentPlayerIndex;

    private PlayerData m_currentPlayer;

    public void Disconnected()
    {
        m_players[(int)m_currentPlayerIndex.Value].IsPlaying = false;
    }

    public void Leave()
    {
        Debug.Log(gameObject.name);

        if (m_currentPlayer.IsPlaying && m_currentPlayer.Character != null && m_characterSelectOpen.Value)
            m_currentPlayer.Character = null;
        else
        {
            m_currentPlayer.Character = null;
            m_currentPlayer.IsPlaying = false;
            m_currentPlayerIndex.Value = -1;
        }
    }

    public void Join(int controllerNum)
    {
        Debug.Log(gameObject.name);
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
                m_currentPlayer = player;
                break;
            }
            index++;
        }
        if (!m_characterSelectOpen.Value)
        {
            foreach (CharacterData character in m_characters)
            {
                if (!character.inUse)
                {
                    m_currentPlayer.Character = character;
                    break;
                }
            }
        }
    }
}
