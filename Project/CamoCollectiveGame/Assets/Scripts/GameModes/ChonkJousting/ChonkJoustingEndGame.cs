using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChonkJoustingEndGame : MonoBehaviour
{
    [SerializeField]
    private List<ChonkJouster> m_jousters;
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
        ChonkJouster winner = null;

        foreach (ChonkJouster jouster in m_jousters)
        {
            if (jouster.player == null)
                continue;
            if (winner == null || winner.score < jouster.score)
                winner = jouster;
        }

        m_winnerText.gameObject.SetActive(true);
        m_winnerText.text = winner.player.GetCharacter().name + " WINS!!!";
        winner.player.AddToScore(1);
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
