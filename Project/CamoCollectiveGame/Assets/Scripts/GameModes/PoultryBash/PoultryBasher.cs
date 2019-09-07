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

    [Header("Drumsticks")]
    [SerializeField]
    private Renderer m_leftDrumstick;
    [SerializeField]
    private Renderer m_rightDrumstick;

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

    private bool m_punchedRight;
    private bool m_punchedLeft;
    private bool m_isPunching;

    private float m_knockbackScale;

    private InputMapper m_input;
    private Animator m_animator;
    private StandardCharacterController m_controller;

    public bool m_leftPunch;

    private bool m_punchQueued;
    private bool m_inRing;
    private Coroutine m_speedCo;
    private Coroutine m_strengthCo;

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
            m_animator.SetFloat("MoveSpeed", m_controller.Velocity);
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
        if (collision.transform.CompareTag("Finish"))
        {
            m_deadValue.Value = true;
            m_input.DisableInput();
            ParticleSystem ps = m_deathParticleSystemPool.GetObject();
            ps.transform.up = (m_rotateTowards - transform.position);
            ps.transform.position = transform.position;
            ps.Play();
            Destroy(gameObject);
        }
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
        m_leftDrumstick.material.color = Color.red;
        m_rightDrumstick.material.color = Color.red;
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
        m_leftDrumstick.material.color = m_playerData.Character.TempColour;
        m_rightDrumstick.material.color = m_playerData.Character.TempColour;
    }

    public void AlternatePunch()
    {
        if (m_leftPunch)
            QueueNextPunch("LeftPunch");
        else
            QueueNextPunch("RightPunch");
    }

    private void QueueNextPunch(string anim)
    {
        if (m_punchQueued)
            return;
        if (!m_inRing)
            return;
        m_animator.SetTrigger(anim);
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
            rb.AddForce(transform.forward * m_knockback * m_knockbackScale, ForceMode.Impulse);
            rb.AddForce(Vector3.up * m_knockup, ForceMode.Impulse);
        }
    }

    public void OnPunchEnd()
    {
        m_punchQueued = false;
        m_leftPunch = !m_leftPunch;
    }

    private void OnDestroy()
    {
        m_gameObjectListSet.Remove(gameObject);
    }
}
