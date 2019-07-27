using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerClimbWin : MonoBehaviour
{
    [SerializeField]
    private List<TowerClimber> m_climbers;
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
        TowerClimber m_winner = null;
        foreach (TowerClimber climber in m_climbers)
        {
            if (climber.player == null || !climber.player.IsPlaying())
                continue;
            alive += Convert.ToInt32(!climber.isDead);
            if (!climber.isDead)
                m_winner = climber;
        }
        if (alive == 1)
        {
            m_winnerText.text = m_winner.player.GetCharacter().name + " Wins!";
            m_winnerText.gameObject.SetActive(true);
            m_winner.player.AddToScore(1);
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
