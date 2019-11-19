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
    private Text m_otherText;
    [SerializeField]
    private GameModeWinText m_winnerText;
    [SerializeField]
    private FloatValue m_speedScaleValue;

    [Header("Data")]
    [SerializeField]
    private BoolValue m_isDeadValues;
    [SerializeField]
    private PlayerDataReference m_prevWinner;
    private bool m_hasWon;

    private void Start()
    {
        m_winnerText.gameObject.SetActive(false);
        m_otherText.gameObject.SetActive(false);
        for (int i = 0; i < 4; i++)
            m_isDeadValues.SetValue(i, false);
    }

    private void Update()
    {
        if (m_hasWon)
        {
            m_speedScaleValue.Value += 10 * Time.deltaTime;
            return;
        }
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
            m_winnerText.gameObject.SetActive(true);
            m_winnerText.SetWinner(winner + 1);
            m_otherText.gameObject.SetActive(false);
            
            m_wonGameEvent.Invoke();
            m_hasWon = true;
            m_prevWinner.value = m_playerData[winner];
        }
        else if (alive == 0)
        {
            m_otherText.text = "Nobody wins I guess?";
            m_winnerText.gameObject.SetActive(false);
            m_otherText.gameObject.SetActive(true);
            m_wonGameEvent.Invoke();
            m_hasWon = true;
        }
    }
}
