using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoultryBashPowerupSpawner : MonoBehaviour
{
    [SerializeField]
    private float m_spawnRadius;
    [SerializeField]
    private float m_spawnForce;
    [SerializeField]
    private GameObject m_powerup;
    [SerializeField]
    private GameObjectEvent m_dynamicCameraAddEvent;
    [SerializeField]
    private GameObjectEvent m_dynamicCameraRemoveEvent;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Vector2 circPoint = Random.insideUnitCircle;
            Vector3 spawnPoint = transform.position + new Vector3(circPoint.x * m_spawnRadius, transform.position.y, circPoint.y * m_spawnRadius);
            Rigidbody rb = Instantiate(m_powerup, spawnPoint, Quaternion.identity, null).GetComponent<Rigidbody>();
            rb.AddForce((transform.position - spawnPoint).normalized * m_spawnForce, ForceMode.Impulse);
            m_dynamicCameraAddEvent.Invoke(rb.gameObject);
        }
    }
}
