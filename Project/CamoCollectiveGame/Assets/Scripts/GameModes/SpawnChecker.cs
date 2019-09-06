using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnChecker : MonoBehaviour
{
    [SerializeField]
    private int m_playerAmmount;
    [SerializeField]
    private List<PlayerData> m_playerData;

    private void Awake()
    {
        int players = 0;
        foreach (PlayerData player in m_playerData)
            players += Convert.ToInt32(player.IsPlaying);
        if (players != m_playerAmmount)
            gameObject.SetActive(false);
    }
}
