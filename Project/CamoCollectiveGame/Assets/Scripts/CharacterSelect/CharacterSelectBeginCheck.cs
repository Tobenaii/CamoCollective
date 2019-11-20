using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SceneLoader))]
public class CharacterSelectBeginCheck : MonoBehaviour
{
    [SerializeField]
    private List<PlayerData> m_players;
    [SerializeField]
    private GameEvent m_closeEvent;
    [SerializeField]
    private BoolValue m_characterSelectOpen;

    private void Awake()
    {
        m_characterSelectOpen.Value = true;
    }

    private void OnDestroy()
    {
        m_characterSelectOpen.Value = false;
    }

    private void Update()
    {
        int isPlaying = 0;
        foreach (PlayerData player in m_players)
            isPlaying += Convert.ToInt32(player.IsPlaying);
    }

    public void Close()
    {
        StartCoroutine(NextFrameCloseCheck());
    }

    private IEnumerator NextFrameCloseCheck()
    {
        //TODO: Fix rid of this sh*t
        yield return null;
        if (m_players[0].IsPlaying == false)
            transform.GetChild(0).GetComponent<SceneLoader>().Load();
    }

    public void Begin()
    {
        bool begin = true;
        foreach (PlayerData player in m_players)
        {
            if (player.IsPlaying && player.Character == null)
            {
                begin = false;
                break;
            }
        }
        int isPlaying = 0;
        foreach (PlayerData player in m_players)
            isPlaying += Convert.ToInt32(player.IsPlaying);
        if (isPlaying > 1 && begin)
        {
            m_closeEvent.Invoke();
            GetComponent<SceneLoader>()?.Load();
        }
    }
}
