using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerClimbDeath : MonoBehaviour
{
    [SerializeField]
    private PlayerData m_playerData;

    [Header("ParticleEffects")]
    [SerializeField]
    private ParticleSystemPool m_particleSystemPool;

    [Header("Sounds")]
    [SerializeField]
    private AudioSource m_onDeathSound;

    [Header("Data")]
    [SerializeField]
    private BoolReference m_isDeadValue;
    [SerializeField]
    private FloatReference m_fallSpeed;

    private float m_speedOffset;
    private Camera m_camera;

    private void Awake()
    {
        m_isDeadValue.Value = false;
        GameObject character = Instantiate(m_playerData.Character.TowerClimbCharacter, transform);
        m_camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vport = m_camera.WorldToViewportPoint(transform.position);
        if (vport.y < 0)
            OnDeath();
    }

    private void LateUpdate()
    {
        Vector3 vport = m_camera.WorldToViewportPoint(transform.position);
        if (vport.y > 0.8f)
        {
            Debug.Log("Increasing Speed");
            m_fallSpeed.Value += 1.0f * Time.deltaTime;
        }
    }

    private void OnDeath()
    {
        m_onDeathSound.transform.SetParent(null);
        m_onDeathSound.Play();
        m_isDeadValue.Value = true;
        ParticleSystem ps = m_particleSystemPool.GetObject();
        ps.transform.position = transform.position;
        ps.Play();
        Destroy(gameObject);
    }
}
