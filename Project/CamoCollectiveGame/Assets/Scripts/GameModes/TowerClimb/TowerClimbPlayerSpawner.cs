﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerClimbPlayerSpawner : MonoBehaviour
{
    [SerializeField]
    private List<Transform> m_spawns;
    [SerializeField]
    private List<PlayerData> m_playerData;
    [SerializeField]
    private List<TowerClimber> m_towerClimbers;
    [SerializeField]
    private GameObject m_playerPrefab;

    private void Awake()
    {
        foreach (TowerClimber climber in m_towerClimbers)
        {
            climber.isDead = true;
        }

        int index = 0;
        foreach (PlayerData player in m_playerData)
        {
            if (!player.IsPlaying())
                continue;
            InputMapper input = Instantiate(m_playerPrefab, m_spawns[index].position, m_spawns[index].rotation, transform).GetComponent<InputMapper>();
            m_towerClimbers[index].player = player;
            m_towerClimbers[index].placement = index;
            m_towerClimbers[index].towerClimber = input.gameObject;
            m_towerClimbers[index].isDead = false;
            input.GetComponent<TowerClimbDeath>().SetClimber(m_towerClimbers[index]);
            input.SetControllerNum(player.GetPlayerNum());
            index++;
        }
    }

    private void Update()
    {
    }
}
