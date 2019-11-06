using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoultryBasher : MonoBehaviour
{
    [Header("Punching")]
    [SerializeField]
    private float m_punchRadius;
    [SerializeField]
    private float m_punchDistance;
    [SerializeField]
    private float m_knockback;
    [SerializeField]
    private float m_knockup;
    [SerializeField]
    private float m_punchHitTime;
    [SerializeField]
    private float m_punchEndTime;
    [SerializeField]
    private float m_punchQueueTime;
    [SerializeField]
    private float m_punchPreezeTime;


    [Header("Controller Vibration")]
    [SerializeField]
    private float m_vibrationTime;
    [SerializeField]
    [Range(0, 1)]
    private float m_vibrationAmount;

    [Header("Blocking")]
    [SerializeField]
    private Renderer m_shield;
    [SerializeField]
    private float m_blockKnockbackScale;
    [SerializeField]
    private float m_blockKnockupScale;
    [SerializeField]
    private float m_blockMoveSpeedMultiplier;
    [SerializeField]
    private float m_blockCooldownAfterPunch;
    [SerializeField]
    [Range(0, 1)]
    private float m_shieldBreakPercentOnHit;
    [SerializeField]
    private float m_shieldRegenRate;
    [SerializeField]
    private float m_shieldRegenDelay;
    [SerializeField]
    private float m_shieldDecayRate;
    [SerializeField]
    private float m_blockKnockbackForce;
    [SerializeField]
    private float m_blockKnockupForce;


    [Header("Particles")]
    [SerializeField]
    private ParticleSystemPool m_deathParticleSystemPool;
    [SerializeField]
    private Vector3 m_rotateTowards;
    [SerializeField]
    private ParticleSystem m_speedParticleSystem;

    private ParticleSystem[] m_punchParticles;

    [Header("Data")]
    [SerializeField]
    private FloatReference m_speedScale;
    [SerializeField]
    private PlayerData m_playerData;
    [SerializeField]
    private BoolReference m_deadValue;
    [SerializeField]
    private GameEvent m_dynamicCameraStartEvent;
    [SerializeField]
    private GameObjectListSet m_gameObjectListSet;
    [SerializeField]
    private GameObject m_playerIndicator;
    [SerializeField]
    private GameEvent m_cameraShakeEvent;

    private float m_currentBlockKnockbackScale;

    private bool m_isPunching;

    private float m_knockbackScale;
    private float m_currentBlockScale;
    private float m_blockCooldownAfterPunchTimer;
    private float m_punchQueueResetTimer;
    private Rigidbody m_rb;

    private InputMapper m_input;
    private Animator m_animator;
    private StandardCharacterController m_controller;

    private bool m_punchQueued;
    private bool m_inRing;
    private Coroutine m_speedCo;
    private Coroutine m_strengthCo;
    private bool m_isBlocking;
    private bool m_regenShield;

    private bool m_leftPunch;
    private TimeLerper m_shieldLerper = new TimeLerper();

    private Rigidbody[] m_rbRagdolls;

    private float m_shieldHealth;
    private Vector3 m_initShieldScale;
    private bool m_shieldBroke;

    private ParticleSystem m_leftSwipeParticles;
    private ParticleSystem m_rightSwipeParticles;

    private void Start()
    {


        m_shieldHealth = 1;
        m_initShieldScale = m_shield.transform.localScale;
        m_controller = GetComponent<StandardCharacterController>();
        m_rb = GetComponent<Rigidbody>();
        Instantiate(m_playerData.Character.PoultryBashCharacter, transform);
        m_gameObjectListSet.Add(gameObject);
        m_input = GetComponent<InputMapper>();
        m_dynamicCameraStartEvent.Invoke();
        //foreach (Renderer rend in GetComponentsInChildren<Renderer>())
        //    rend.material.color = m_playerData.Character.TempColour;
        m_animator = GetComponentInChildren<Animator>();
        m_animator.enabled = true;

        m_speedScale.Value = 1;
        m_knockbackScale = 1;
        m_rbRagdolls = GetComponentsInChildren<Rigidbody>();
        for (int i = 1; i < m_rbRagdolls.Length; i++)
        {
            m_rbRagdolls[i].useGravity = false;
            m_rbRagdolls[i].detectCollisions = false;
            m_rbRagdolls[i].isKinematic = true;
        }
        m_punchParticles = gameObject.GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem ps in m_punchParticles)
        {
            if (ps.transform.CompareTag("LeftSwipe"))
                m_leftSwipeParticles = ps;
            else if (ps.transform.CompareTag("RightSwipe"))
                m_rightSwipeParticles = ps;
        }
        //Instantiate(m_playerData.Character.PoultryBashCharacter, transform);
    }

    private void Update()
    {
        if (m_animator)
        {
            m_animator.SetFloat("MoveSpeed", m_controller.Speed, 1f, Time.deltaTime * 10f);
            //m_animator.SetFloat("StrafeSpeed", Mathf.Lerp(1,0, Mathf.Abs(Vector3.Dot(transform.forward, m_controller.Velocity))) * m_controller.Speed, 1f, Time.deltaTime * 10f);
        }

        if (m_punchQueueResetTimer > 0)
            m_punchQueueResetTimer -= Time.deltaTime;

        if (m_regenShield)
        {
            m_shieldHealth += m_shieldRegenRate * Time.deltaTime;
            m_shieldHealth = Mathf.Clamp(m_shieldHealth, 0, 1);
        }
        m_shield.transform.localScale = m_initShieldScale * m_shieldHealth;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ring"))
            m_inRing = true;
        else if (other.CompareTag("Powerup"))
        {
            PoultryBashPowerup powerup = other.GetComponentInParent<PoultryBashPowerup>();
            powerup.TriggerEnter(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ring"))
            m_inRing = false;
        else if (other.CompareTag("Powerup"))
        {
            PoultryBashPowerup powerup = other.GetComponentInParent<PoultryBashPowerup>();
            powerup.TriggerExit(this);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            PoultryBashPowerup powerup = other.GetComponentInParent<PoultryBashPowerup>();
            powerup.TriggerStay(this);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Stop"))
            OnDeath();
    }

    private void OnDeath()
    {
        m_deadValue.Value = true;
        m_input.DisableInput();
        for (int i = 1; i < m_rbRagdolls.Length; i++)
        {
            m_rbRagdolls[i].useGravity = true;
            m_rbRagdolls[i].detectCollisions = true;
            m_rbRagdolls[i].isKinematic = false;
        }
        m_animator.enabled = false;
        m_rb.detectCollisions = false;
        m_rb.isKinematic = true;
        EndBlock();
        m_playerIndicator.SetActive(false);
        //ParticleSystem ps = m_deathParticleSystemPool.GetObject();
        //ps.transform.up = (m_rotateTowards - transform.position);
        //ps.transform.position = transform.position;
        //ps.Play();
        //Destroy(gameObject);
        StartCoroutine(RemoveCameraDelay());
    }

    private IEnumerator RemoveCameraDelay()
    {
        yield return new WaitForSeconds(1);
        m_gameObjectListSet.Remove(gameObject);
    }

    public void ScaleSpeed(float scale)
    {
        m_speedScale.Value = scale;
    }

    public void ScaleSpeedForSeconds(float scale, float seconds)
    {
        if (m_speedCo != null)
            StopCoroutine(m_speedCo);
        m_speedScale.Value = scale;
        m_speedCo = StartCoroutine(ResetSpeedScale(seconds));
    }

    public void ScaleKnockbackForSeconds(float scale, float seconds)
    {
        if (m_strengthCo != null)
            StopCoroutine(m_strengthCo);
        m_knockbackScale = scale;
        m_strengthCo = StartCoroutine(ResetKnockbackScale(seconds));
    }

    private IEnumerator ResetSpeedScale(float seconds)
    {
        float timer = seconds;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        m_speedScale.Value = 1;
    }

    public void Freeze(float seconds, Vector3 newVelocity)
    {
        StartCoroutine(FreezeForSeconds(seconds, newVelocity));
    }

    private IEnumerator FreezeForSeconds(float seconds, Vector3 newVelocity)
    {
        float prevSpeed = m_animator.speed;
        m_animator.speed = prevSpeed * 0.25f;

        float timer = seconds;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        m_animator.speed = prevSpeed;
        if (newVelocity != Vector3.zero)
            m_rb.velocity = newVelocity;
    }

    private IEnumerator ResetKnockbackScale(float seconds)
    {
        float timer = seconds;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        m_knockbackScale = 1;
    }

    private void QueueNextPunch(string anim)
    {
        if (m_isPunching)
        {
            m_punchQueued = true;
            m_punchQueueResetTimer = m_punchQueueTime;
            return;
        }
        if (!m_inRing)
            return;
        m_animator.SetTrigger(anim);
        if (m_leftPunch)
            m_leftSwipeParticles.Play();
        else
            m_rightSwipeParticles.Play();
        Debug.Log(anim);
        StopCoroutine(PunchHitTimer());
        StartCoroutine(PunchHitTimer());

        StopCoroutine(PunchEndTimer());
        StartCoroutine(PunchEndTimer());
    }

    private IEnumerator PunchHitTimer()
    {
        m_isPunching = true;
        float timer = m_punchHitTime;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        Punch(0);
    }

    private IEnumerator PunchEndTimer()
    {
        float timer = m_punchEndTime;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        OnPunchEnd();
    }

    public void OnPunchEnd()
    {
        m_isPunching = false;
        if (m_leftPunch)
            m_leftSwipeParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        else
            m_rightSwipeParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        m_leftPunch = !m_leftPunch;
        if (m_punchQueued && m_punchQueueResetTimer > 0)
            AlternatePunch(1);
    }

    public void LeftPunch(float trigger)
    {
        if (trigger == 0 || m_isBlocking)
            return;

        m_animator.ResetTrigger("RightPunch");
        QueueNextPunch("LeftPunch");
    }

    public void RightPunch(float trigger)
    {
        if (trigger == 0 || m_isBlocking)
            return;

        m_animator.ResetTrigger("LeftPunch");
        QueueNextPunch("RightPunch");
    }

    public void AlternatePunch(float trigger)
    {
        if (trigger == 0 || m_isBlocking)
            return;

        if (m_leftPunch)
            LeftPunch(1);
        else
            RightPunch(1);
    }


    public void Punch(float rightOffset)
    {
        if (!m_inRing)
            return;
        m_blockCooldownAfterPunchTimer = m_blockCooldownAfterPunch;
        RaycastHit hit;
        if (Physics.CapsuleCast(((transform.position - transform.up / 2) - transform.forward * m_punchRadius) + transform.right * rightOffset, (transform.position + transform.up / 2) - transform.forward * m_punchRadius, m_punchRadius, transform.forward, out hit, m_punchDistance, 1 << 10))
        {
            Rigidbody rb = hit.transform.GetComponent<Rigidbody>();
            PoultryBasher pb = hit.transform.GetComponent<PoultryBasher>();
            if (pb.m_isBlocking)
            {
                pb.m_shieldHealth -= m_shieldBreakPercentOnHit;
                rb.velocity = (pb.transform.forward * pb.m_blockKnockbackForce) + (Vector3.up * pb.m_blockKnockupForce);
            }
            rb.velocity = Vector3.zero;
            float dot = Vector3.Dot(transform.forward, rb.transform.forward);
            float knockbackScale = (dot < -0.15f) ? pb.m_currentBlockKnockbackScale : 1;
            Vector3 newVelocity = (transform.forward * m_knockback * m_knockbackScale * knockbackScale) + (Vector3.up * m_knockup);
            hit.transform.GetComponent<InputMapper>().Vibrate(m_vibrationTime, m_vibrationAmount, m_vibrationAmount);
            rb.velocity = newVelocity;
            m_cameraShakeEvent.Invoke();
            //Freeze(m_punchPreezeTime, Vector3.zero);
            //pb.Freeze(m_punchPreezeTime, newVelocity);
        }
    }

    public void StartBlock(float trigger)
    {
        if (m_shieldHealth <= 0)
        {
            if (m_isBlocking)
            {
                m_shieldBroke = true;
                EndBlock();
            }
            return;
        }
        if (m_blockCooldownAfterPunchTimer > 0)
        {
            m_blockCooldownAfterPunchTimer -= Time.deltaTime;
            return;
        }
        if (trigger > 0)
        {
            if (m_isPunching || m_shieldBroke)
                return;
            if (!m_isBlocking)
            {
                m_shieldLerper.Reset();
                StartCoroutine(FadeInShield());
            }
            m_shieldHealth -= m_shieldDecayRate * Time.deltaTime;
            m_shieldHealth = Mathf.Clamp(m_shieldHealth, 0, 1);
            m_regenShield = false;
            m_isBlocking = true;
            m_speedScale.Value = m_blockMoveSpeedMultiplier;
            m_currentBlockKnockbackScale = m_blockKnockbackScale;
            m_animator.ResetTrigger("StopDefend");
            m_animator.SetTrigger("StartDefend");
            m_shield.gameObject.SetActive(true);
        }
        else if (m_isBlocking)
        {
            EndBlock();
        }
        else
            m_shieldBroke = false;
    }

    private IEnumerator ShieldRegenDelay()
    {
        float timer = m_shieldRegenDelay;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        m_regenShield = true;
    }

    private IEnumerator FadeInShield()
    {
        float time = 0.5f;
        float timer = time;
        Color c = m_shield.material.color;
        m_shield.material.color = new Color(c.r, c.g, c.b, 0);
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            c.a = m_shieldLerper.Lerp(0, 0.75f, time);
            m_shield.material.color = new Color(c.r, c.g, c.b, c.a);
            yield return null;
        }
    }

    public void EndBlock()
    {
        StopCoroutine(ShieldRegenDelay());
        StartCoroutine(ShieldRegenDelay());
        m_shield.gameObject.SetActive(false);
        m_speedScale.Value = 1;
        m_currentBlockKnockbackScale = 1;
        m_animator.ResetTrigger("StartDefend");
        m_animator.SetTrigger("StopDefend");
        m_isBlocking = false;
        StopCoroutine(FadeInShield());
    }

    private void OnDestroy()
    {
        m_gameObjectListSet.Remove(gameObject);
    }
}
