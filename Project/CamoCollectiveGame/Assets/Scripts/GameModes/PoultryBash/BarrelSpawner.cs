using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelSpawner : MonoBehaviour
{
    [Header("Random")]
    [SerializeField]
    private float m_spawnRadius;
    [SerializeField]
    private float m_minSpawnTime;
    [SerializeField]
    private float m_maxSpawnTime;

    [Header("Spiral")]
    [SerializeField]
    private float m_spiralSpawnSpeed;
    [SerializeField]
    private float m_barrelOffset;
    [SerializeField]
    private float m_spiralOffset;
    [SerializeField]
    private int m_maxBarrels;

    [SerializeField]
    private GameObject m_barrelPrefab;

    private float m_timer;
    private bool m_spawn;
    private bool m_spiralSpawn;

    private float m_spiralRadius;
    private float m_spiralAngle;
    private float m_curSpiralOffset;
    private float m_curBarrelAmmount;

    private TimeLerper m_lerper = new TimeLerper();


    private void Update()
    {
        if (!m_spawn)
            return;

        if (m_spiralSpawn)
        {
            Vector3 pos = new Vector3(Mathf.Sin(m_spiralAngle * Mathf.Deg2Rad), 0, Mathf.Cos(m_spiralAngle * Mathf.Deg2Rad)) * (m_spiralRadius - m_curSpiralOffset);
            GameObject barrel = Instantiate(m_barrelPrefab);
            barrel.transform.position = transform.position + pos;
            barrel.transform.position = transform.position + pos;
            m_spiralAngle += m_barrelOffset;
            m_curSpiralOffset += m_spiralOffset;
            m_curBarrelAmmount++;
            if (m_curBarrelAmmount >= m_maxBarrels)
                m_spawn = false;
            return;
        }

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

    public void SpiralSpawn()
    {
        m_spiralSpawn = true;
        m_spiralRadius = m_spawnRadius;
    }

    private void GetRandomTime()
    {
        m_timer = Random.Range(m_minSpawnTime, m_maxSpawnTime);
    }

    public void StartSpawning()
    {
        GetRandomTime();
        m_spawn = true;
        //SpiralSpawn();
    }

    public void StopSpawning()
    {
        m_spawn = false;
    }
}
