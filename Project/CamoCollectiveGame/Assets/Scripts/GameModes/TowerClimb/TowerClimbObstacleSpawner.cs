using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerClimbObstacleSpawner : MonoBehaviour
{
    [System.Serializable]
    private class Obstacle
    {
        public int spawnChance;
        public GameObjectPool objectPool;
    }

    [SerializeField]
    private FloatReference m_moveSpeed;
    [SerializeField]
    private float m_spawnFrequency;
    [SerializeField]
    private float m_extents;
    [SerializeField]
    private List<Obstacle> m_obstacles;
    [SerializeField]
    private BoolValue m_moveValue;

    private float m_frequencyTimer;
    private bool m_spawn;

    private void OnValidate()
    {
        for (int i = 0; i < m_obstacles.Count; i++)
        {
            if (i != 0)
            {
                if (m_obstacles[i].spawnChance < m_obstacles[i - 1].spawnChance)
                {
                    Debug.LogWarning("Range must be higher than previous item");
                    m_obstacles[i].spawnChance = m_obstacles[i - 1].spawnChance + 1;
                }
            }
        }
    }

    public void StartSpawning()
    {
        m_spawn = true;
        m_frequencyTimer = m_spawnFrequency;
    }


    public void StopSpawning()
    {
        m_spawn = false;
    }

    public void StartMoving()
    {
        m_moveValue.Value = true;
    }

    private void Update()
    {
        if (!m_spawn)
            return;

        m_frequencyTimer -= Time.deltaTime;
        if (m_frequencyTimer <= 0)
        {
            m_frequencyTimer = m_spawnFrequency;
            int spawnValue = Random.Range(0, 101);

            GameObject obstacle = null;

            foreach (Obstacle obs in m_obstacles)
            {
                if (spawnValue < obs.spawnChance)
                {
                    obstacle = obs.objectPool.GetObject();
                    break;
                }
            }

            if (obstacle != null)
            {
                obstacle.transform.rotation = transform.rotation;
                obstacle.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + Random.Range(-m_extents - 1, m_extents + 1));
                obstacle.transform.SetParent(transform);
            }
        }
    }
}
