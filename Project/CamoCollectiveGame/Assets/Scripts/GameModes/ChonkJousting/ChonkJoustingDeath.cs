using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChonkJoustingData))]
public class ChonkJoustingDeath : MonoBehaviour
{
    [SerializeField]
    private float m_respawnTime;
    [SerializeField]
    private int m_scoreLossOnDeath;
    private float m_respawnTimer;
    private bool m_isRespawning;

    private void Update()
    {
        if (!m_isRespawning)
            return;
        m_respawnTimer -= Time.deltaTime;
        if (m_respawnTimer <= 0)
            Respawn();
    }

    private void Respawn()
    {
        m_isRespawning = false;
        GetComponent<ChonkJoustingData>().ResetLives();
        GetComponent<ChonkJoustingController>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().detectCollisions = true;
    }

    public void OnDeath()
    {
        GetComponent<ChonkJoustingController>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().detectCollisions = false;
        GetComponent<ChonkJoustingData>().RemoveScore(m_scoreLossOnDeath);
        m_respawnTimer = m_respawnTime;
        m_isRespawning = true;
    }

}
