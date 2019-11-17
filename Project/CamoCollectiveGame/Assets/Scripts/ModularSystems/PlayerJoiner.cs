using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJoiner : MonoBehaviour
{
    [SerializeField]
    private int m_controllerNum;
    [SerializeField]
    private FloatValue m_mainController;
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
    [SerializeField]
    private BoolValue m_storyModeIsPlaying;

    private PlayerData m_currentPlayer;

    private void Start()
    {
        if (m_currentPlayerIndex.Value != -1)
            m_currentPlayer = m_players[(int)m_currentPlayerIndex.Value];
        if (m_controllerNum == m_mainController.Value)
            Join(m_controllerNum);
    }

    public void Disconnected()
    {
        m_players[(int)m_currentPlayerIndex.Value].IsPlaying = false;
    }

    public void Leave()
    {
        if (m_currentPlayer == null || (!m_characterSelectOpen.Value && m_storyModeIsPlaying.Value))
            return;

        if (m_currentPlayer.IsPlaying && m_currentPlayer.Character != null && m_characterSelectOpen.Value)
            m_currentPlayer.Character = null;
        else if (m_controllerNum != 1)
        {
            m_currentPlayer.Character = null;
            m_currentPlayer.IsPlaying = false;
            m_currentPlayerIndex.Value = -1;
        }
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
                player.JustJoined = true;
                m_currentPlayerIndex.Value = index;
                m_playerControllers.SetValue(index, controllerNum);
                m_currentPlayer = player;
                break;
            }
            index++;
        }
        //if (!m_characterSelectOpen.Value)
        //{
        //    foreach (CharacterData character in m_characters)
        //    {
        //        if (!character.inUse)
        //        {
        //            m_currentPlayer.Character = character;
        //            break;
        //        }
        //    }
        //}
    }
}
