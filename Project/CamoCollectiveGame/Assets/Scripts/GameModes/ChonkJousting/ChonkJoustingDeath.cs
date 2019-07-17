using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChonkJoustingDeath : MonoBehaviour
{
    [SerializeField]
    private float m_respawnTime;
    [SerializeField]
    private float m_initLives;
    [SerializeField]
    private float m_scoreLossOnDeath;
    [SerializeField]
    private FloatValue m_livesValue;
    [SerializeField]
    private FloatValue m_scoreValue;
    private float m_respawnTimer;
    private bool m_isRespawning;

    private void Awake()
    {
        m_scoreValue.value = 0;
        m_livesValue.value = m_initLives;
    }

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
        m_livesValue.value = m_initLives;
        GetComponent<ChonkJoustingController>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().detectCollisions = true;
    }

    public void OnDeath()
    {
        GetComponent<ChonkJoustingController>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().detectCollisions = false;
        m_scoreValue.value -= m_scoreLossOnDeath;
        m_respawnTimer = m_respawnTime;
        m_isRespawning = true;
    }

}
