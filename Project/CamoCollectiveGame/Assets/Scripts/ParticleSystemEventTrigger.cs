using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemEventTrigger : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem m_particleSystem;

    public void TriggerParticles()
    {
        m_particleSystem.Play();
    }
}
