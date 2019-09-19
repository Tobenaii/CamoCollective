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
    private float m_punchTime;

    [Header("Blocking")]
    [SerializeField]
    private float m_blockKnockbackScale;
    [SerializeField]
    private float m_blockKnockupScale;
    [SerializeField]
    private float m_blockMoveSpeedMultiplier;

    [Header("Particles")]
    [SerializeField]
    private ParticleSystemPool m_deathParticleSystemPool;
    [SerializeField]
    private Vector3 m_rotateTowards;
    [SerializeField]
    private ParticleSystem m_speedParticleSystem;

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

    private float m_currentBlockKnockbackScale;

    private bool m_punchedRight;
    private bool m_punchedLeft;
    private bool m_isPunching;

    private float m_knockbackScale;
    private float m_currentBlockScale;

    private InputMapper m_input;
    private Animator m_animator;
    private StandardCharacterController m_controller;

    private bool m_punchQueued;
    private bool m_inRing;
    private Coroutine m_speedCo;
    private Coroutine m_strengthCo;

    private bool m_leftPunch;

    private void Start()
    {
        m_controller = GetComponent<StandardCharacterController>();
        Instantiate(m_playerData.Character.PoultryBashCharacter, transform);
        m_gameObjectListSet.Add(gameObject);
        m_input = GetComponent<InputMapper>();
        m_dynamicCameraStartEvent.Invoke();
        //foreach (Renderer rend in GetComponentsInChildren<Renderer>())
        //    rend.material.color = m_playerData.Character.TempColour;
        m_animator = GetComponentInChildren<Animator>();

        m_speedScale.Value = 1;
        m_knockbackScale = 1;
        //Instantiate(m_playerData.Character.PoultryBashCharacter, transform);
    }

    private void Update()
    {
        if (m_animator)
        {
            m_animator.SetFloat("MoveSpeed", m_controller.Speed, 1f, Time.deltaTime * 10f);
            m_animator.SetFloat("StrafeSpeed", Mathf.Lerp(1,0, Mathf.Abs(Vector3.Dot(transform.forward, m_controller.Velocity))) * m_controller.Speed, 1f, Time.deltaTime * 10f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stop"))
            m_inRing = true;
        else if (other.CompareTag("Powerup"))
        {
            PoultryBashPowerup powerup = other.GetComponentInParent<PoultryBashPowerup>();
            powerup.TriggerEnter(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Stop"))
        {
            OnDeath();
            m_inRing = false;
        }
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

    }

    private void OnDeath()
    {
        m_deadValue.Value = true;
        m_input.DisableInput();
        ParticleSystem ps = m_deathParticleSystemPool.GetObject();
        ps.transform.up = (m_rotateTowards - transform.position);
        ps.transform.position = transform.position;
        ps.Play();
        Destroy(gameObject);
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
        if (m_punchQueued)
            return;
        if (!m_inRing)
            return;
        m_animator.SetTrigger(anim);
        StopCoroutine(PunchTimer());
        StartCoroutine(PunchTimer());
    }

    public void LeftPunch(float trigger)
    {
        if (trigger == 0)
        {
            m_punchedLeft = false;
            return;
        }
        if (m_punchedLeft)
            return;
        m_punchedLeft = true;
        QueueNextPunch("LeftPunch");
    }

    public void RightPunch(float trigger)
    {
        if (trigger == 0)
        {
            m_punchedRight = false;
            return;
        }
        if (m_punchedRight)
            return;
        m_punchedRight = true;
        QueueNextPunch("RightPunch");
    }

    public void Punch(float rightOffset)
    {
        if (!m_inRing)
            return;
        RaycastHit hit;
        if (Physics.CapsuleCast(((transform.position - transform.up / 2) - transform.forward * m_punchRadius) + transform.right * rightOffset, (transform.position + transform.up / 2) - transform.forward * m_punchRadius, m_punchRadius, transform.forward, out hit, m_punchDistance, 1<<10))
        {
            Rigidbody rb = hit.transform.GetComponent<Rigidbody>();
            PoultryBasher pb = hit.transform.GetComponent<PoultryBasher>();
            rb.velocity = Vector3.zero;
            rb.AddForce(transform.forward * m_knockback * m_knockbackScale * pb.m_currentBlockKnockbackScale, ForceMode.Impulse);
            rb.AddForce(Vector3.up * m_knockup, ForceMode.Impulse);
        }
    }

    private IEnumerator PunchTimer()
    {
        m_punchQueued = true;
        float timer = m_punchTime;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        Punch(0);
        m_leftPunch = !m_leftPunch;
        m_punchQueued = false;
    }

    public void AlternatePunch()
    {
        if (m_leftPunch)
            LeftPunch(1);
        else
            RightPunch(1);
    }

    public void StartBlock(float trigger)
    {
        if (trigger > 0)
        {
            m_speedScale.Value = m_blockMoveSpeedMultiplier;
            m_currentBlockKnockbackScale = m_blockKnockbackScale;
            m_animator.ResetTrigger("StopDefend");
            m_animator.SetTrigger("StartDefend");
        }
        else
            EndBlock();
    }

    public void EndBlock()
    {
        m_speedScale.Value = 1;
        m_currentBlockKnockbackScale = 1;
        m_animator.ResetTrigger("StartDefend");
        m_animator.SetTrigger("StopDefend");
    }

    public void OnPunchEnd()
    {
        //m_punchQueued = false;
        //m_leftPunch = !m_leftPunch;
        m_leftPunch = !m_leftPunch;
    }

    private void OnDestroy()
    {
        m_gameObjectListSet.Remove(gameObject);
    }
}
