using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChonkJoustingDeath : MonoBehaviour
{
    [SerializeField]
    private int m_initLives;
    [SerializeField]
    private float m_respawnTime;
    [SerializeField]
    private int m_scoreLossOnDeath;
    [SerializeField]
    private float m_fadeAwayTime;
    [SerializeField]
    private GameObjectEvent m_respawnEvent;
    [SerializeField]
    private PlayerData m_playerData;
    [SerializeField]
    private Renderer m_tempBody;
    private float m_respawnTimer;
    private bool m_isRespawning;
    private Animator m_animator;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
        m_animator.enabled = false;
        m_playerData.ChonkJoustingData.lives = m_initLives;

        GameObject character = Instantiate(m_playerData.Character.ChonkJoustingCharacter, transform);
        character.transform.localPosition = Vector3.zero;
        character.transform.localRotation = Quaternion.identity;
    }

    private void Update()
    {
        if (!m_isRespawning)
            return;
        m_respawnTimer -= Time.deltaTime;
        m_playerData.ChonkJoustingData.respawnTimer = m_respawnTimer;
        if (m_respawnTimer <= 0)
            Respawn();
    }

    private void Respawn()
    {
        m_isRespawning = false;
        m_playerData.ChonkJoustingData.lives = m_initLives;
        GetComponent<ChonkJoustingController>().enabled = true;
        m_playerData.ChonkJoustingData.isDead = false;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().detectCollisions = true;
        m_respawnEvent.Invoke(gameObject);
        m_animator.enabled = false;

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in renderers)
        {
            rend.enabled = true;
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
        m_playerData.ChonkJoustingData.isDead = true;
        m_playerData.ChonkJoustingData.score -= m_scoreLossOnDeath;
        if (m_playerData.ChonkJoustingData.score < 0)
            m_playerData.ChonkJoustingData.score = 0;
        m_respawnTimer = m_respawnTime;
        m_isRespawning = true;
        m_animator.enabled = true;
        StartCoroutine(FadeAway());
    }

    IEnumerator FadeAway()
    {
        TimeLerper lerper = new TimeLerper();
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in renderers)
        {
            rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }
        float fadeTimer = m_fadeAwayTime;
        while (fadeTimer > 0)
        {
            fadeTimer -= Time.deltaTime;
            foreach (Renderer rend in renderers)
            {
                Color color = rend.material.color;
                float dissolve = lerper.Lerp(0, 1, m_fadeAwayTime);
                rend.material.SetFloat("_Amount", dissolve);
            }
            yield return null;
        }
        m_animator.SetTrigger("EndAnim");
        foreach (Renderer rend in renderers)
        {
            rend.enabled = false;
        }
    }
}
