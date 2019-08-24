using System;
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
    private float m_numberOfWins;
    [SerializeField]
    private FloatValue m_scoreValues;
    [SerializeField]
    private GameEvent m_newRoundEvent;

    private bool m_wonGame;
    private int m_winner;

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
        if (m_winner >= 0 && m_scoreValues.GetValue(m_winner) == m_numberOfWins)
        {
            m_winnerText.gameObject.SetActive(true);
            m_winnerText.text = m_players[m_winner].Character.name + " WINS THE ENTIRE GAME!!!";
            m_finishedEvent.Invoke();
            return;
        }
        Debug.Log("ROUND: " + (m_roundNumberValue.Value + 1));
        StartCoroutine(WinnerText(m_winner, alive));
    }

    IEnumerator WinnerText(int winner, int alive)
    {
        m_winnerText.gameObject.SetActive(true);
        if (alive == 0)
            m_winnerText.text = "NOBODY WINS!!!";
        else
        {
            m_winnerText.text = m_players[winner].Character.name + " WINS!!!";
            m_scoreValues.SetValue(winner, m_scoreValues.GetValue(winner) + 1);
        }
        float timer = 3;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        m_newRoundEvent.Invoke();
    }
}
