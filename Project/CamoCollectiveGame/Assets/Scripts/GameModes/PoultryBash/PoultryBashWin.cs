using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private bool m_wonGame;

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
        int alive = 0;
        int winner = -1;
        for (int i = 0; i < 4; i++)
        {
            if (!m_players[i].IsPlaying)
                continue;
            bool dead = m_deadValues.GetValue(i);
            alive += Convert.ToInt32(!m_deadValues.GetValue(i));
            if (!dead)
                winner = i;
        }

        if (alive > 1 || winner == -1)
            return;
        m_winnerText.gameObject.SetActive(true);
        m_winnerText.text = m_players[winner].Character.name + " WINS!!!";
        m_wonGame = true;
        m_finishedEvent.Invoke();
    }
}
