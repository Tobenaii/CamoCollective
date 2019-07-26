using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChonkJoustingDeath : MonoBehaviour
{
    [SerializeField]
    private float m_respawnTime;
    [SerializeField]
    private int m_scoreLossOnDeath;
    [SerializeField]
    private float m_fadeAwayTime;
    [SerializeField]
    private Transform m_respawnTransform;
    private float m_respawnTimer;
    private bool m_isRespawning;
    private Animator m_animator;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
        m_animator.enabled = false;
    }
    public void SetRespawnTransform(Transform transform)
    {
        m_respawnTransform = transform;
    }

    private void Update()
    {
        if (!m_isRespawning)
            return;
        m_respawnTimer -= Time.deltaTime;
        GetComponent<ChonkJoustingData>().SetRespawnTimer(m_respawnTimer);
        if (m_respawnTimer <= 0)
            Respawn();
    }

    private void Respawn()
    {
        m_isRespawning = false;
        GetComponent<ChonkJoustingData>().ResetLives();
        GetComponent<ChonkJoustingController>().enabled = true;
        GetComponent<ChonkJoustingController>().Respawn(m_respawnTransform);
        GetComponent<ChonkJoustingData>().SetDead(false);
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().detectCollisions = true;
        m_animator.enabled = false;

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in renderers)
        {
            Color color = rend.material.color;
            rend.material.SetFloat("_Amount", 0);
            rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
    }

    public void OnDeath()
    {
        GetComponent<ChonkJoustingController>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().detectCollisions = false;
        GetComponent<ChonkJoustingData>().RemoveScore(m_scoreLossOnDeath);
        GetComponent<ChonkJoustingData>().SetDead(true);
        m_respawnTimer = m_respawnTime;
        m_isRespawning = true;
        m_animator.enabled = true;
        StartCoroutine(FadeAway());
    }

    IEnumerator FadeAway()
    {
        TimeLerper lerper = new TimeLerper();
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        float fadeTimer = m_fadeAwayTime;
        while (fadeTimer > 0)
        {
            fadeTimer -= Time.deltaTime;
            foreach (Renderer rend in renderers)
            {
                Color color = rend.material.color;
                float dissolve = lerper.Lerp(0, 1, m_fadeAwayTime);
                rend.material.SetFloat("_Amount", dissolve);
                rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            }
            yield return null;
        }
        m_animator.SetTrigger("EndAnim");
    }

}
