using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChonkJoustingEndGame : MonoBehaviour
{
    [SerializeField]
    private GameModeWinText m_winnerText;
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
    private BoolValue m_fullyDeadValue;
    [SerializeField]
    private GameEvent m_joustingFinishedEvent;
    [SerializeField]
    private PlayerDataReference m_prevWinner;

    private bool m_wonGame;

    private void Awake()
    {
        m_winnerText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (m_wonGame)
            return;
        int winner = -1;
        int index = 0;
        foreach (PlayerData player in m_players)
        {
            if (player.IsPlaying)
            {
                if (!m_fullyDeadValue.GetValue(index))
                {
                    if (winner != -1)
                        return;
                    winner = index;
                }
            }
            index++;
        }
        EndGame(winner);
    }

    //We're in the end game now
    public void EndGame(int winner)
    {
        m_wonGame = true;
        m_winnerText.gameObject.SetActive(true);
        m_winnerText.SetWinner(winner + 1);
        m_prevWinner.value = m_players[winner];
        StartCoroutine(FinishUpGame());
        m_joustingFinishedEvent.Invoke();
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
