﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChonkJouster : MonoBehaviour
{
    [Header("Shield")]
    [SerializeField]
    private float m_shieldAngle;
    [SerializeField]
    private float m_shieldKnockbackForce;

    [Header("Lance")]
    [SerializeField]
    private Transform m_lanceTransform;
    [SerializeField]
    private float m_knockbackForce;
    [SerializeField]
    private float m_knockupForce;
    [SerializeField]
    private float m_invincibilityFrame;
    [SerializeField]
    private float m_invincibilityFlashTime;

    [Header("Death")]
    [SerializeField]
    private float m_fadeAwayTime;

    [Header("Respawn")]
    [SerializeField]
    private float m_respawnTime;

    [Header("Data")]
    [SerializeField]
    private PlayerData m_player;
    [SerializeField]
    private FloatReference m_scoreValue;
    [SerializeField]
    private FloatReference m_livesValue;
    [SerializeField]
    private FloatReference m_respawnTimer;
    [SerializeField]
    private GameObjectEvent m_respawnEvent;
    [SerializeField]
    private GameEvent m_respawnedEvent;

    //Components
    private Rigidbody m_rb;
    private StandardCharacterController m_controller;

    //State
    private bool m_isInvincible;
    private bool m_isRespawning;

    void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_controller = GetComponent<StandardCharacterController>();
    }

    private void Start()
    {
        GameObject character = Instantiate(m_player.Character.ChonkJoustingCharacter, transform);
        character.transform.localPosition = Vector3.zero;
        character.transform.localRotation = Quaternion.identity;
        m_livesValue.Reset();
        m_scoreValue.Reset();
    }

    void Update()
    {
        if (m_isRespawning)
        {
            m_respawnTimer.Value -= Time.deltaTime;
            if (m_respawnTimer.Value <= 0)
                Respawn();
            return;
        }

        //Raycast from lance transform to determine if a player was hit
        //RaycastHit hit;
        //if (Physics.Raycast(new Ray(m_lanceTransform.position, m_lanceTransform.forward), out hit, 1))
        //{
        //    if (hit.transform.CompareTag("Player"))
        //    {
        //        ChonkJouster jouster = hit.transform.GetComponentInParent<ChonkJouster>();
        //        if (jouster.m_isInvincible)
        //            return;
        //        CheckAttack(jouster);
        //    }
        //}
    }

    //Check if lance hit shield
    public void CheckAttack(ChonkJouster jouster)
    {
        //If the other jouster is respawning, ignore it
        if (jouster.m_isRespawning)
            return;
        float dot = Vector3.Dot(transform.forward, jouster.transform.forward);
        float shield = Mathf.Lerp(-1, 1, Mathf.InverseLerp(0, 180, m_shieldAngle));
        //Did hit shield
        if (dot < shield)
        {
            jouster.Knockback(transform.forward * m_shieldKnockbackForce);
            Knockback(transform.forward * -1 * m_shieldKnockbackForce);
        }
        //Didn't hit shield
        else
        {
            m_scoreValue.Value++;
            jouster.GetHurt(transform.forward * m_knockbackForce, transform.up * m_knockupForce);
        }
    }

    public void GetHurt(Vector3 knockbackForce, Vector3 knockupForce)
    {
        //Lost points and a life
        Knockback(knockbackForce);
        Knockback(knockupForce);
        m_livesValue.Value--;
        if (m_livesValue.Value <= 0)
            Die();
        else
        {
            StartCoroutine(InvincibilityFrame(m_invincibilityFrame));
            StartCoroutine(Flash(m_invincibilityFrame, m_invincibilityFlashTime));
        }
    }

    public void Knockback(Vector3 force)
    {
        m_rb.AddForce(force, ForceMode.Impulse);
    }

    public void Die()
    {
        //Disable input and start fading away
        m_controller.enabled = false;
        m_respawnTimer.Value = m_respawnTime;
        StartCoroutine(FadeAway());
        m_isRespawning = true;
    }

    public void Respawn()
    {
        m_respawnEvent.Invoke(gameObject);
        m_isRespawning = false;
        m_livesValue.Reset();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Lance"))
        {
            if (m_isInvincible)
                return;
            ChonkJouster jouster = other.transform.GetComponentInParent<ChonkJouster>();
            jouster.CheckAttack(this);
        }

        else if (other.CompareTag("Respawner"))
        {
            m_controller.enabled = true;
            m_respawnedEvent.Invoke();
        }
    }

    private IEnumerator FadeAway()
    {
        //Set fade away ammount in shader to 1 over time
        TimeLerper lerper = new TimeLerper();
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer rend in renderers)
            rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        float fadeTimer = m_fadeAwayTime;
        while (fadeTimer > 0)
        {
            fadeTimer -= Time.deltaTime;
            foreach (Renderer rend in renderers)
            {
                float dissolve = lerper.Lerp(0, 1, m_fadeAwayTime);
                rend.material.SetFloat("_Amount", dissolve);
            }
            yield return null;
        }
        transform.position += Vector3.up * 1000;
        yield return null;
        foreach (Renderer rend in renderers)
        {
            rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            rend.material.SetFloat("_Amount", 0);
        }
    }

    private IEnumerator Flash(float time, float flashTime)
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        Color[] colours = new Color[renderers.Length];

        int i = 0;
        foreach (Renderer rend in renderers)
        {
            colours[i] = rend.material.color;
            i++;
        }

        m_isInvincible = true;
        float timer = time;
        float flashTimer = flashTime;
        bool flash = true;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            flashTimer -= Time.deltaTime;

            i = 0;
            foreach (Renderer rend in renderers)
            {
                if (flash)
                {
                    rend.material.color = colours[i];
                    rend.material.color = new Color(rend.material.color.r, rend.material.color.g, rend.material.color.b, 0.4f);
                }
                else
                    rend.material.color = new Color(1, 0, 0, 0.3f);
                i++;
            }
            if (flashTimer <= 0)
            {
                flash = !flash;
                flashTimer = m_invincibilityFlashTime;
            }

            yield return null;
        }
        i = 0;
        foreach (Renderer rend in renderers)
        {
            rend.material.color = colours[i];
            i++;
        }
    }

    private IEnumerator InvincibilityFrame(float time)
    {
        float timer = time;
        while (timer > 0)
        {
            m_isInvincible = true;
            timer -= Time.deltaTime;
            yield return null;
        }
        m_isInvincible = false;
    }
}