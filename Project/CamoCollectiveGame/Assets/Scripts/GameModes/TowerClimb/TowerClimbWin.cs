using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerClimbWin : MonoBehaviour
{
    [SerializeField]
    private List<PlayerData> m_playerData;
    [SerializeField]
    private GameEvent m_wonGameEvent;
    [SerializeField]
    private Text m_winnerText;

    [Header("Data")]
    [SerializeField]
    private BoolValue m_isDeadValues;
    [SerializeField]
    private FloatValue m_rulesScores;
    private bool m_hasWon;

    private void Start()
    {
        for (int i = 0; i < 4; i++)
            m_isDeadValues.Value = false;
    }

    private void Update()
    {
        if (m_hasWon)
            return;
        int alive = 0;

        int winner = -1;
        int index = -1;
        foreach (PlayerData player in m_playerData)
        {
            index++;
            if (!player.IsPlaying)
                continue;
            alive += Convert.ToInt32(!m_isDeadValues.GetValue(index));
            if (!m_isDeadValues.GetValue(index))
                winner = index;
        }
        if (alive == 1)
        {
            m_winnerText.text = m_playerData[winner].Character.name + " Wins!";
            m_winnerText.gameObject.SetActive(true);
            
            m_wonGameEvent.Invoke();
            m_hasWon = true;

        }
        else if (alive == 0)
        {
            m_winnerText.text = "Nobody wins I guess?";
            m_winnerText.gameObject.SetActive(true);
            m_wonGameEvent.Invoke();
            m_hasWon = true;
        }
    }
}
