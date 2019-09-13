using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeReadyUpCheck : MonoBehaviour
{
    [SerializeField]
    private List<PlayerData> m_players;
    [SerializeField]
    private GameEvent m_everyoneReadyEvent;

    private int m_playersPlaying;
    private int m_playersReady;

    private void Start()
    {
        foreach (PlayerData player in m_players)
            m_playersPlaying += Convert.ToInt32(player.IsPlaying);
    }

    public void ReadyUpCheck()
    {
        m_playersReady++;
        if (m_playersReady == m_playersPlaying)
            m_everyoneReadyEvent.Invoke();
    }
}
