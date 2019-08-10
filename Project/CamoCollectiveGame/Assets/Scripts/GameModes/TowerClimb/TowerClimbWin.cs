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
    private bool m_hasWon;

    private void Update()
    {
        if (m_hasWon)
            return;
        int alive = 0;
        PlayerData m_winner = null;
        foreach (PlayerData player in m_playerData)
        {
            if (!player.IsPlaying)
                continue;
            alive += Convert.ToInt32(!player.TowerClimbData.isDead);
            if (!player.TowerClimbData.isDead)
                m_winner = player;
        }
        if (alive == 1)
        {
            m_winnerText.text = m_winner.Character.name + " Wins!";
            m_winnerText.gameObject.SetActive(true);
            m_winner.RulerScore++;
            StartCoroutine(WonGame());
            m_hasWon = true;
            
        }
        else if (alive == 0)
        {
            m_winnerText.text = "Nobody wins I guess?";
            m_winnerText.gameObject.SetActive(true);
            StartCoroutine(WonGame());
            m_hasWon = true;
        }
    }

    private IEnumerator WonGame()
    {
        yield return new WaitForSeconds(0.1f);
        m_wonGameEvent.Invoke();
    }
}
