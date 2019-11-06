using System.Collections;
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
    [SerializeField]
    private float m_attackFrequency;

    [Header("Colliders")]
    [SerializeField]
    private List<Collider> m_colliders;

    [Header("Controller Vibration")]
    [SerializeField]
    private float m_vibrationTime;
    [SerializeField]
    [Range(0, 1)]
    private float m_vibrationAmount;

    [Header("Particles")]
    [SerializeField]
    private ParticleSystemPool m_gainPointsParticles;
    private List<ParticleSystem> m_activeParticles = new List<ParticleSystem>();

    [Header("Death")]
    [SerializeField]
    private float m_fadeAwayTime;
    [SerializeField]
    private int m_scoreLossOnDeath;

    [Header("Respawn")]
    [SerializeField]
    private float m_respawnTime;

    [Header("Mud")]
    [SerializeField]
    private float m_speedScaleInMud;

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
    [SerializeField]
    private FloatReference m_chonkSpeedScale;
    [SerializeField]
    private BoolReference m_isDeadValue;
    [SerializeField]
    private BoolReference m_fullyDeadValue;

    private Rigidbody[] m_rbRagdolls;
    private Collider[] m_colRagdolls;
    private CharacterJoint[] m_jointRagdolls;

    //Components
    private Rigidbody m_rb;
    private StandardCharacterController m_controller;
    private Animator m_animator;
    private InputMapper m_input;

    //State
    private bool m_isInvincible;
    private bool m_isRespawning;
    private bool m_triggeredRespawn;

    private float m_attackFrequencyTimer;


    void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_controller = GetComponent<StandardCharacterController>();
        m_input = GetComponent<InputMapper>();
    }

    private void Start()
    {
        GameObject character = Instantiate(m_player.Character.ChonkJoustingCharacter, transform);
        m_animator = GetComponentInChildren<Animator>();
        character.transform.localPosition = Vector3.zero;
        character.transform.localRotation = Quaternion.identity;
        m_livesValue.Reset();
        m_scoreValue.Reset();
        m_chonkSpeedScale.Value = 1;
        m_isDeadValue.Value = false;
        m_fullyDeadValue.Value = false;
        m_rbRagdolls = GetComponentsInChildren<Rigidbody>();
        for (int i = 1; i < m_rbRagdolls.Length; i++)
        {
            m_rbRagdolls[i].useGravity = false;
            m_rbRagdolls[i].detectCollisions = false;
            m_rbRagdolls[i].isKinematic = true;
        }
    }

    public void Reinit()
    {
        Destroy(transform.GetChild(0));
        GameObject character = Instantiate(m_player.Character.ChonkJoustingCharacter, transform);
        m_animator = GetComponentInChildren<Animator>();
        character.transform.localPosition = Vector3.zero;
        character.transform.localRotation = Quaternion.identity;

        m_chonkSpeedScale.Value = 1;
        m_rbRagdolls = GetComponentsInChildren<Rigidbody>();
        for (int i = 1; i < m_rbRagdolls.Length; i++)
        {
            m_rbRagdolls[i].useGravity = false;
            m_rbRagdolls[i].detectCollisions = false;
            m_rbRagdolls[i].isKinematic = true;
        }
    }

    void Update()
    {
        if (m_animator)
            m_animator.SetFloat("RunSpeedMult", m_controller.Speed);

        for (int i = 0; i < m_activeParticles.Count; i++)
        {
            if (m_activeParticles[i].isStopped)
            {
                m_gainPointsParticles.DestroyObject(m_activeParticles[i]);
                m_activeParticles.RemoveAt(i);
                i--;
            }
        }

        if (m_isRespawning && !m_fullyDeadValue.Value)
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
        if (m_attackFrequencyTimer > 0)
            m_attackFrequencyTimer -= Time.deltaTime;
    }

    private void ToggleColliders(bool enable)
    {
        foreach (Collider col in m_colliders)
            col.enabled = enable;
    }

    //Check if lance hit shield
    public void CheckAttack(ChonkJouster jouster)
    {
        //If the other jouster is respawning, ignore it
        if (jouster.m_isRespawning || m_attackFrequencyTimer > 0 || m_controller.Velocity == Vector3.zero)
            return;
        m_attackFrequencyTimer = m_attackFrequency;
        float dot = Vector3.Dot(transform.forward, jouster.transform.forward);
        float shield = Mathf.Lerp(-1, 1, Mathf.InverseLerp(0, 180, m_shieldAngle));
        //Did hit shield
        if (dot < shield)
        {
            Debug.Log("HIT!");
            jouster.Knockback(transform.forward * m_shieldKnockbackForce);
            Knockback(transform.forward * -1 * m_shieldKnockbackForce);
        }
        //Didn't hit shield
        else
        {
            m_scoreValue.Value++;
            ParticleSystem ps = m_gainPointsParticles.GetObject();
            ps.transform.position = transform.position;
            m_activeParticles.Add(ps);
            ps.Play();
            jouster.GetHurt(transform.forward * m_knockbackForce, transform.up * m_knockupForce);
        }
    }

    public void GetHurt(Vector3 knockbackForce, Vector3 knockupForce)
    {
        //Lost points and a life
        Knockback(knockbackForce);
        Knockback(knockupForce);

        m_input.Vibrate(m_vibrationTime, m_vibrationAmount, m_vibrationAmount);

        m_livesValue.Value--;
        if (m_livesValue.Value <= 0)
            m_fullyDeadValue.Value = true;
        Die();

        
        //else
        //{
        //    StartCoroutine(InvincibilityFrame(m_invincibilityFrame));
        //    StartCoroutine(Flash(m_invincibilityFrame, m_invincibilityFlashTime, new Color(1,0,0,0.3f)));
        //}
    }

    public void Knockback(Vector3 force)
    {
        m_rb.velocity = force;
        //m_rb.AddForce(force, ForceMode.Impulse);
    }

    public void Die()
    {
        //Disable input and start fading away
        m_controller.enabled = false;
        m_respawnTimer.Value = m_respawnTime;
        m_scoreValue.Value -= m_scoreLossOnDeath;
        if (m_scoreValue.Value < 0)
            m_scoreValue.Value = 0;
        //StartCoroutine(FadeAway());
        m_isRespawning = true;
        m_triggeredRespawn = false;
        //m_rb.detectCollisions = false;
        //m_animator.SetTrigger("Die");
        m_isDeadValue.Value = true;
        m_animator.enabled = false;
        if (m_fullyDeadValue.Value)
        {
            m_rb.detectCollisions = false;
            return;
        }
        ToggleColliders(false);
        for (int i = 1; i < m_rbRagdolls.Length; i++)
        {
            m_rbRagdolls[i].useGravity = true;
            m_rbRagdolls[i].detectCollisions = true;
            m_rbRagdolls[i].isKinematic = false;
        }
        //StartCoroutine(ToggleRagdoll());
    }

    public void Respawn()
    {
        m_chonkSpeedScale.Value = 1;
        m_rb.isKinematic = true;
        m_respawnEvent.Invoke(gameObject);
        m_isRespawning = false;
        //m_livesValue.Reset();
        m_isDeadValue.Value = false;
        for (int i = 1; i < m_rbRagdolls.Length; i++)
        {
            m_rbRagdolls[i].useGravity = false;
            m_rbRagdolls[i].detectCollisions = false;
            m_rbRagdolls[i].isKinematic = true;
        }
        m_animator.enabled = true;
        ToggleColliders(true);
    }

    private IEnumerator ToggleRagdoll()
    {
        //float timer = 0.533f;
        float timer = 0;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Lance"))
        {
            if (m_isInvincible)
                return;
            ChonkJouster jouster = other.transform.GetComponentInParent<ChonkJouster>();
            jouster.CheckAttack(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Respawner"))
        {
            if (!m_triggeredRespawn)
            {
                m_triggeredRespawn = true;
                m_controller.enabled = true;
                m_rb.useGravity = true;
                m_respawnedEvent.Invoke();
                StartCoroutine(InvincibilityFrame(m_invincibilityFrame));
                StartCoroutine(Flash(m_invincibilityFrame, m_invincibilityFlashTime, new Color(1, 1, 1, 0.7f)));
            }
        }

        else if (other.CompareTag("Mud"))
            m_chonkSpeedScale.Value = m_speedScaleInMud;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Mud"))
            m_chonkSpeedScale.Value = 1;
    }

    List<Material> m_materials = new List<Material>();

    private IEnumerator Flash(float time, float flashTime, Color colour)
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        
        m_materials.Clear();

        foreach (Renderer rend in renderers)
        {
            foreach (Material mat in rend.materials)
            {
                m_materials.Add(mat);
            }
        }

        m_isInvincible = true;
        float timer = time;
        float flashTimer = flashTime;
        bool flash = true;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            flashTimer -= Time.deltaTime;

            foreach (Material mat in m_materials)
            {
                if (flash)
                    mat.SetColor("_EmissionColor", Color.black);
                else
                    mat.SetColor("_EmissionColor", colour);
            }
            if (flashTimer <= 0)
            {
                flash = !flash;
                flashTimer = m_invincibilityFlashTime;
            }

            yield return null;
        }
        foreach (Material mat in m_materials)
            mat.SetColor("_EmissionColor", Color.black);
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

    private void OnMouseDown()
    {
        Die();
    }
}
