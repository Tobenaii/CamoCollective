﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PoultryBashWin : MonoBehaviour
{
    [SerializeField]
    private List<PlayerData> m_players;
    [SerializeField]
    private BoolValue m_deadValues;
    [SerializeField]
    private Text m_winnerText;
    [SerializeField]
    private GameEvent m_finishedEvent;
    [SerializeField]
    private float m_winnerCheckInterval;
    [SerializeField]
    private FloatValue m_roundNumberValue;
    [SerializeField]
    private FloatValue m_scoreValues;
    [SerializeField]
    private GameEvent m_newRoundEvent;
    [SerializeField]
    private BoolValue m_spawnTempPlayers;

    private bool m_wonGame;
    private int m_winner;
    private List<PlayerData> m_highestPlayers = new List<PlayerData>();

    private void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            m_deadValues.SetValue(i, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_wonGame)
            return;
        int alive = GetAliveCount();

        if (alive > 1)
            return;
        StartCoroutine(DrawCheck());
    }

    private int GetAliveCount()
    {
        int alive = 0;
        m_winner = -1;
        for (int i = 0; i < 4; i++)
        {
            if (!m_players[i].IsPlaying)
                continue;
            bool dead = m_deadValues.GetValue(i);
            alive += Convert.ToInt32(!m_deadValues.GetValue(i));
            if (!dead)
                m_winner = i;
        }
        return alive;
    }

    private IEnumerator DrawCheck()
    {
        m_wonGame = true;
        float checkTimer = m_winnerCheckInterval;
        while (checkTimer > 0)
        {
            checkTimer -= Time.deltaTime;
            yield return null;
        }
        int winner = -1;
        WinGame(GetAliveCount());
    }

    private void WinGame(int alive)
    {
        m_wonGame = true;
        m_roundNumberValue.Value++;
        m_scoreValues.SetValue(m_winner, m_scoreValues.GetValue(m_winner) + 1);

        m_highestPlayers.Clear();

        for (int i = 0; i < m_scoreValues.Count; i++)
        {
            if (m_scoreValues.GetValue(i) == 1)
                m_players[i].TempIsPlaying = true;
            else if (m_scoreValues.GetValue(i) == 2)
            {
                WinGame();
                return;
            }
        }

        if (m_roundNumberValue.Value == 3)
        {
            m_spawnTempPlayers.Value = true;
            StartCoroutine(WinRound("Tie Breaker!"));
            return;
        }

        if (alive == 0)
            StartCoroutine(WinRound("Nobody Wins!"));
        else
            StartCoroutine(WinRound("PLAYER " + (m_winner + 1) + " WINS THE ROUND!"));
    }

    private void WinGame()
    {
        m_winnerText.gameObject.SetActive(true);
        m_winnerText.text = "PLAYER " + (m_winner + 1) + " WINS THE GAME!";
        foreach (PlayerData player in m_players)
            player.TempIsPlaying = false;
        m_spawnTempPlayers.Value = false;
        m_finishedEvent.Invoke();
    }

    IEnumerator WinRound(string text)
    {
        m_winnerText.gameObject.SetActive(true);
        m_winnerText.text = text;
        float timer = 3;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        m_newRoundEvent.Invoke();
    }
}
