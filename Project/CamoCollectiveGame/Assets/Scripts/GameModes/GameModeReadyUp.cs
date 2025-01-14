﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModeReadyUp : MonoBehaviour
{
    [SerializeField]
    private int m_playerNum;
    [SerializeField]
    private PlayerData m_player;
    [SerializeField]
    private GameEvent m_playerReadyEvent;
    [SerializeField]
    private Text m_playerStatusText;
    private bool m_ready;

    private void Awake()
    {
        m_playerStatusText.color = m_player.IndicatorColour;
    }

    public void ReadyUp()
    {
        if (m_ready)
            return;
        m_playerReadyEvent.Invoke();
        m_playerStatusText.text = "Player " + m_playerNum + " Ready!";
        m_ready = true;
    }
}
