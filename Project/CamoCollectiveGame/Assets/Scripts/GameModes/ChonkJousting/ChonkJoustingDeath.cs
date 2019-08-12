using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChonkJoustingDeath : MonoBehaviour
{
    [SerializeField]
    private int m_scoreLossOnDeath;
    [SerializeField]
    private float m_fadeAwayTime;
    [SerializeField]
    private GameObjectEvent m_respawnEvent;
    [SerializeField]
    private Renderer m_tempBody;
    private bool m_isRespawning;
    private Animator m_animator;

    [Header("Data")]
    [SerializeField]
    private PlayerData m_player;
    [SerializeField]
    private FloatReference m_livesValue;
    [SerializeField]
    private FloatReference m_respawnTimer;
    [SerializeField]
    private FloatReference m_scoreValue;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
        m_animator.enabled = false;
        GameObject character = Instantiate(m_player.Character.ChonkJoustingCharacter, transform);
        character.transform.localPosition = Vector3.zero;
        character.transform.localRotation = Quaternion.identity;
        m_livesValue.Reset();
        m_scoreValue.Reset();
    }

    private void Update()
    {
        if (!m_isRespawning)
            return;
        m_respawnTimer.Value -= Time.deltaTime;
        if (m_respawnTimer.Value <= 0)
            Respawn();
    }

    private void Respawn()
    {
        m_isRespawning = false;
        m_livesValue.Reset();
        GetComponent<ChonkJoustingController>().enabled = true;
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

        m_scoreValue.Value -= m_scoreLossOnDeath;
        if (m_scoreValue.Value < 0)
            m_scoreValue.Value = 0;
        m_respawnTimer.Reset();
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
