using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChonkJoustingEndGame : MonoBehaviour
{
    [SerializeField]
    private List<PlayerData> m_players;
    [SerializeField]
    private Text m_winnerText;
    [SerializeField]
    private float m_timeToQuit;
    [SerializeField]
    private GameEvent m_quitGameEvent;

    private void Awake()
    {
        m_winnerText.gameObject.SetActive(false);
    }

    //We're in the end game now
    public void EndGame()
    {
        PlayerData winner = null;

        foreach (PlayerData player in m_players)
        {
            if (!player.IsPlaying)
                continue;
            if (winner == null || winner.ChonkJoustingData.score < player.ChonkJoustingData.score)
                winner = player;
        }

        m_winnerText.gameObject.SetActive(true);
        m_winnerText.text = winner.Character.name + " WINS!!!";
        winner.RulerScore++;
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
