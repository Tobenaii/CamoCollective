using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGame : MonoBehaviour
{
    [SerializeField]
    private FloatValue m_rulerScore;
    [SerializeField]
    private FloatValue m_joinedPlayerIndex;
    [SerializeField]
    private List<PlayerData> m_players;

    private void Awake()
    {
        m_rulerScore.Reset();
        m_joinedPlayerIndex.Reset();
        foreach (PlayerData player in m_players)
        {
            player.TempIsPlaying = false;
            player.IsPlaying = false;
            player.Character = null;
        }
    }
}
