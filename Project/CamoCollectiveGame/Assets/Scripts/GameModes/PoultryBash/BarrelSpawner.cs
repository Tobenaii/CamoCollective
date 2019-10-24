﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelSpawner : MonoBehaviour
{
    [SerializeField]
    private float m_spawnRadius;
    [SerializeField]
    private float m_minSpawnTime;
    [SerializeField]
    private float m_maxSpawnTime;
    [SerializeField]
    private GameObject m_barrelPrefab;

    private float m_timer;
    private bool m_spawn;

    private void Update()
    {
        if (!m_spawn)
            return;
        m_timer -= Time.deltaTime;
        if (m_timer < 0)
        {
            GameObject barrel = Instantiate(m_barrelPrefab);
            barrel.transform.SetParent(transform);
            Vector2 unitCirc = Random.insideUnitCircle * m_spawnRadius;
            Vector3 randDir = new Vector3(unitCirc.x, 0, unitCirc.y);
            barrel.transform.position = transform.position + randDir;
            GetRandomTime();
        }
    }

    private void GetRandomTime()
    {
        m_timer = Random.Range(m_minSpawnTime, m_maxSpawnTime);
    }

    public void StartSpawning()
    {
        GetRandomTime();
        m_spawn = true;
    }

    public void StopSpawning()
    {
        m_spawn = false;
    }
}