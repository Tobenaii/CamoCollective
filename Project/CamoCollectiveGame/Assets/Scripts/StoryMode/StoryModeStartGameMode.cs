using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryModeStartGameMode : MonoBehaviour
{
    [SerializeField]
    private float m_startNextGameModeTime;
    [SerializeField]
    private List<GameEvent> m_startGameModeEvents;
    [SerializeField]
    private BoolValue m_storyModeIsPlaying;

    private int m_gameModeIndex;

    private void Start()
    {
        StartNextGameMode();
        Shuffle();
        m_storyModeIsPlaying.Value = true;
    }

    private void OnDestroy()
    {
        m_storyModeIsPlaying.Value = false;
    }

    public void StartNextGameMode()
    {
        StartCoroutine(StartGameMode());
    }

    private IEnumerator StartGameMode()
    {
        yield return new WaitForSeconds(m_startNextGameModeTime);
        m_startGameModeEvents[m_gameModeIndex].Invoke();
        m_gameModeIndex++;
        if (m_gameModeIndex == m_startGameModeEvents.Count)
        {
            m_gameModeIndex = 0;
            Shuffle();
        }
    }

    private void Shuffle()
    {
        int count = m_startGameModeEvents.Count;
        int last = count - 1;
        for (int i = 0; i < last; i++)
        {
            int r = Random.Range(i, count);
            GameEvent tmp = m_startGameModeEvents[i];
            m_startGameModeEvents[i] = m_startGameModeEvents[r];
            m_startGameModeEvents[r] = tmp;
        }
    }
}
