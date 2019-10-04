using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChonkJoustingEndGame : MonoBehaviour
{
    [SerializeField]
    private Text m_winnerText;
    [SerializeField]
    private float m_timeToQuit;
    [SerializeField]
    private GameEvent m_quitGameEvent;

    [Header("Data")]
    [SerializeField]
    private List<PlayerData> m_players;
    [SerializeField]
    private FloatValue m_scoreValues;
    [SerializeField]
    private FloatValue m_rulerScoreValue;

    private void Awake()
    {
        m_winnerText.gameObject.SetActive(false);
    }

    //We're in the end game now
    public void EndGame()
    {
        int winner = -1;

        int index = 0;
        foreach (PlayerData player in m_players)
        {
            if (player.IsPlaying && (winner == -1 || m_scoreValues.GetValue(index) > m_scoreValues.GetValue(winner)))
                winner = index;
            index++;
        }

        m_winnerText.gameObject.SetActive(true);
        m_winnerText.text = "PLAYER " + (winner + 1) + " WINS!";
        m_rulerScoreValue.SetValue(winner, m_rulerScoreValue.GetValue(winner) + 1);
        StartCoroutine(FinishUpGame());
    }

    IEnumerator FinishUpGame()
    {
        float timer = m_timeToQuit;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        m_quitGameEvent.Invoke();
    }
}
