using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemPoolDeath : MonoBehaviour
{
    [SerializeField]
    private ParticleSystemPool m_pool;

    private ParticleSystem m_particleSystem;

    private void Start()
    {
        m_particleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_particleSystem.isStopped)
            m_pool.DestroyObject(m_particleSystem);
    }
}
