using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SceneLoader))]
public class EnoughPlayersCheck : MonoBehaviour
{
    [SerializeField]
    private BoolValue m_isLoadingValue;
    [SerializeField]
    private List<PlayerData> m_players;
    private SceneLoader m_sceneLoader;
    private bool m_isLoading;

    private void Start()
    {
        m_sceneLoader = GetComponent<SceneLoader>();
    }

    private void LateUpdate()
    {
        if (m_isLoading || m_isLoadingValue.Value)
            return;
        int players = 0;
        foreach (PlayerData player in m_players)
        {
            players += Convert.ToInt32(player.IsPlaying);
        }
        if (players == 1)
        {
            m_isLoading = true;
            m_sceneLoader.Load();
        }

    }
}
