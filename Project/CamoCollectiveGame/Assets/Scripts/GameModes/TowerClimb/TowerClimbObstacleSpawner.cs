﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerClimbObstacleSpawner : MonoBehaviour
{
    [SerializeField]
    private float m_moveSpeed;
    [SerializeField]
    private List<GameObjectPool> m_obstaclePools;
    [SerializeField]
    private float m_spawnFrequency;
    [SerializeField]
    private float m_spawnChance;
    [SerializeField]
    private float m_extents;
    private float m_frequencyTimer;
    private bool m_spawn;

    private List<GameObject> m_obstacles = new List<GameObject>();

    public void StartSpawning()
    {
        m_spawn = true;
        m_frequencyTimer = m_spawnFrequency;
    }

    public void StopSpawning()
    {
        m_spawn = false;
    }

    private void Update()
    {
        if (!m_spawn)
            return;

        m_frequencyTimer -= Time.deltaTime;
        if (m_frequencyTimer <= 0)
        {
            m_frequencyTimer = m_spawnFrequency;
            int spawn = Random.Range(1, 101);
            if (spawn < m_spawnChance)
            {
                int index = Random.Range(0, m_obstaclePools.Count);
                GameObject obstacle = m_obstaclePools[index].GetObject();
                obstacle.transform.position = new Vector3(transform.position.x + Random.Range(-m_extents, m_extents), transform.position.y, transform.position.z);
                obstacle.transform.SetParent(transform);
                m_obstacles.Add(obstacle);
            }
        }

        for (int i = 0; i < m_obstacles.Count; i++)
        {
            GameObject obstacle = m_obstacles[i];
            obstacle.transform.position = new Vector3(obstacle.transform.position.x, obstacle.transform.position.y - m_moveSpeed * Time.deltaTime, obstacle.transform.position.z);
        }
    }
}