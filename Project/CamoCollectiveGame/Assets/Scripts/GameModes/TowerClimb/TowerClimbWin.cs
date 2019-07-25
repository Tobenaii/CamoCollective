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
    private GameEvent m_quitGameEvent;
    [SerializeField]
    private Text m_winnerText;
    [SerializeField]
    private float m_winnerTextTime;
    private bool m_hasWon;

    private void Update()
    {
        if (m_hasWon)
            return;
        int alive = 0;
        TowerClimber m_winner = null;
        foreach (TowerClimber climber in m_climbers)
        {
            if (!climber.player.IsPlaying())
                continue;
            alive += Convert.ToInt32(!climber.isDead);
            if (!climber.isDead)
                m_winner = climber;
        }
        if (alive == 1)
        {
            m_winnerText.text = m_winner.player.GetCharacter().name + " Wins!";
            m_winnerText.gameObject.SetActive(true);
            StartCoroutine(QuitGame());
            m_hasWon = true;
        }
        else if (alive == 0)
        {
            m_winnerText.text = "Nobody wins I guess?";
            m_winnerText.gameObject.SetActive(true);
            StartCoroutine(QuitGame());
            m_hasWon = true;
        }
    }

    private IEnumerator QuitGame()
    {
        float time = m_winnerTextTime;
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        m_quitGameEvent.Invoke();
    }
}
