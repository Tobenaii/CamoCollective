using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChonkJoustingLanceAttack : MonoBehaviour
{
    [SerializeField]
    private float m_knockbackForce;
    [SerializeField]
    private float m_knockupForce;
    [SerializeField]
    private float m_shieldAngle;
    [SerializeField]
    private float m_invincibilityFrame;
    [SerializeField]
    private float m_invincibilityFlashTime;
    private bool m_isInvincible;

    [Header("Joust")]
    [SerializeField]
    private float m_maxJoustAngle;
    [SerializeField]
    private float m_joustRotateSpeed;

    private Vector3 m_targetAim;

    [Header("Data")]
    [SerializeField]
    private FloatReference m_scoreValue;
    [SerializeField]
    private FloatReference m_livesValue;


    private void Update()
    {
        RotateLanceOverTime();

        RaycastHit hit;
        if (Physics.Raycast(new Ray(transform.position, transform.forward), out hit, 1))
        {
            if (hit.transform.CompareTag("Player"))
            {
                ChonkJoustingLanceAttack chonk = hit.transform.GetComponentInParent<ChonkJoustingLanceAttack>();
                if (chonk.m_isInvincible)
                    return;
                Attack(hit.transform.gameObject);
            }
        }
    }

    public void Attack(GameObject other)
    {
        float dot = Vector3.Dot(transform.forward, other.transform.forward);
        float shield = Mathf.Lerp(-1, 1, Mathf.InverseLerp(0, 180, m_shieldAngle));
        if (dot < shield)
        {
            Knockback(transform.forward * -1 * m_knockbackForce * 2, 0);
            other.GetComponentInParent<ChonkJoustingLanceAttack>().Knockback(transform.forward * m_knockbackForce * 2, 0);
        }
        else
        {
            m_scoreValue.Value++;
            other.GetComponentInParent<ChonkJoustingLanceAttack>().Hurt(transform.forward * m_knockbackForce, m_knockupForce);
        }
    }

    public void Aim(Vector2 joystick)
    {
        m_targetAim = new Vector3(joystick.x, 0, joystick.y);
        m_targetAim = Quaternion.AngleAxis(90, Vector3.up) * m_targetAim;
    }

    public void RotateLanceOverTime()
    {
        Vector3 maxNegRot = Quaternion.AngleAxis(-m_maxJoustAngle, Vector3.up) * transform.forward;
        Vector3 maxPosRot = Quaternion.AngleAxis(m_maxJoustAngle, Vector3.up) * transform.forward;

        if (m_targetAim == Vector3.zero)
            return;
        Quaternion prevRot = transform.rotation;
        transform.forward = m_targetAim;
        Quaternion newRot = transform.transform.rotation;
        transform.rotation = prevRot;
        float targetDot = Vector3.Dot(m_targetAim, transform.right);
        float frwdDot = Vector3.Dot(transform.forward, transform.right);
        float angleNormal = Mathf.InverseLerp(90, 0, m_maxJoustAngle);
        if (targetDot < angleNormal && frwdDot < angleNormal)
            return;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRot, m_joustRotateSpeed);
    }


    public void Knockback(Vector3 knockback, float knockup)
    {
        GetComponentInParent<Rigidbody>().AddForce(Vector3.up * knockup, ForceMode.Impulse);
        GetComponentInParent<Rigidbody>().AddForce(knockback, ForceMode.Impulse);
    }

    public void Hurt(Vector3 knockback, float knockup)
    {
        Knockback(knockback, knockup);
        m_livesValue.Value--;
        if (m_livesValue.Value <= 0)
            GetComponent<ChonkJoustingDeath>().OnDeath();
        else
            StartCoroutine(InvincibilityFrame());
    }

    private IEnumerator InvincibilityFrame()
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
        float timer = m_invincibilityFrame;
        float flashTimer = m_invincibilityFlashTime;
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
        m_isInvincible = false;
        i = 0;
        foreach (Renderer rend in renderers)
        {
            rend.material.color = colours[i];
            i++;
        }
    }
}
