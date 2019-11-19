using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicEnoughPlayersCheck : MonoBehaviour
{
    [SerializeField]
    private GameObject m_gameObject;
    [SerializeField]
    private List<PlayerData> m_players;

    private void Update()
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
            m_gameObject.SetActive(true);
        else
            m_gameObject.SetActive(false);
    }
}
