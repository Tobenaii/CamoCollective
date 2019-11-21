using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoryModeScore : MonoBehaviour
{
    [SerializeField]
    private FloatValue m_rulerScores;
    [SerializeField]
    private PlayerDataReference m_prevWinner;
    [SerializeField]
    private GameEvent m_startNextGameModeEvent;
    [SerializeField]
    private GameEvent m_storyModeWonEvent;
    [SerializeField]
    private GameModeWinText m_winnerText;

    private void Start()
    {
        m_prevWinner.value = null;
    }

    public void UpdateScore()
    {
        if (m_prevWinner.value == null)
            return;
        m_prevWinner.value.Character.GainedPointEvent.Invoke();
        int index = m_prevWinner.value.PlayerNumber - 1;
        m_rulerScores.SetValue(index, m_rulerScores.GetValue(index) + 1);
        if (m_rulerScores.GetValue(index) == 3)
            StartCoroutine(WinnerText(index + 1));
        else
            m_startNextGameModeEvent.Invoke();
        m_prevWinner.value = null;
    }

    IEnumerator WinnerText(int winner)
    {
        m_winnerText.gameObject.SetActive(true);
        m_winnerText.SetWinner(winner);
        yield return new WaitForSeconds(5);
        m_storyModeWonEvent.Invoke();
    }
}
