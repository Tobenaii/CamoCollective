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

    private List<GameObject> m_liveObstacles = new List<GameObject>();
    private List<Obstacle> m_possibleObstacles = new List<Obstacle>();
    private float m_frequencyTimer;
    private bool m_spawn;
    private bool m_move;

    private void OnValidate()
    {
        int percents = 0;
        foreach (Obstacle o in m_obstacles)
            percents += o.spawnChance;
        if (percents > 100)
        {
            Debug.LogWarning("Collective percentage cannot be over 100");
            m_obstacles[m_obstacles.Count - 1].spawnChance = 0;
        }
    }

    public void StartSpawning()
    {
        m_spawn = true;
        m_frequencyTimer = m_spawnFrequency;
    }

    public void Destruct()
    {
        Destroy(gameObject);
    }

    public void StopSpawning()
    {
        m_spawn = false;
    }

    public void StartMoving()
    {
        m_move = true;
    }

    private void Update()
    {
        if (!m_spawn)
            return;

        m_frequencyTimer -= Time.deltaTime;
        if (m_frequencyTimer <= 0)
        {
            m_frequencyTimer = m_spawnFrequency;
            int spawnValue = Random.Range(1, 101);

            m_possibleObstacles.Clear();

            foreach (Obstacle obstacle in m_obstacles)
            {
                if (obstacle.spawnChance < spawnValue)
                    m_possibleObstacles.Add(obstacle);
            }

            if (m_possibleObstacles.Count > 0)
            {
                int randObstacle = Random.Range(0, m_possibleObstacles.Count);

                GameObject obstacleObj = m_possibleObstacles[randObstacle].objectPool.GetObject();
                obstacleObj.transform.rotation = transform.rotation;
                obstacleObj.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + Random.Range(-m_extents, m_extents));
                obstacleObj.transform.SetParent(transform);
                m_liveObstacles.Add(obstacleObj);
            }
        }

        if (!m_move)
            return;
        for (int i = 0; i < m_liveObstacles.Count; i++)
        {
            GameObject obstacle = m_liveObstacles[i];
            obstacle.transform.position += Vector3.down * m_moveSpeed.Value * Time.deltaTime;
        }
    }
}
