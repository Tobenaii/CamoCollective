using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoultryBashPowerupSpawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObjectPool> m_powerupPools;
    [SerializeField]
    private float m_spawnRadius;
    [SerializeField]
    private float m_spawnForce;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 circPoint = Random.insideUnitCircle;
            Vector3 spawnPoint = transform.position + new Vector3(circPoint.x * m_spawnRadius, transform.position.y, circPoint.y * m_spawnRadius);

            Rigidbody rb = m_powerupPools[Random.Range(0, m_powerupPools.Count)].GetObject().GetComponent<Rigidbody>();
            rb.transform.position = spawnPoint;

            rb.AddForce((transform.position - spawnPoint).normalized * m_spawnForce, ForceMode.Impulse);
        }
    }
}
