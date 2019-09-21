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
        if (begin)
        {
            m_closeEvent.Invoke();
            GetComponent<SceneLoader>().Load();
        }
    }
}
