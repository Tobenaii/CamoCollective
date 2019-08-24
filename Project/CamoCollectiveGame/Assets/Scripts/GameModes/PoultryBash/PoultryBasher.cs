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

    [Header("Animation")]
    [SerializeField]
    private Animator m_animator;

    [Header("Data")]
    [SerializeField]
    private FloatReference m_speedScale;
    [SerializeField]
    private PlayerData m_playerData;
    [SerializeField]
    private BoolReference m_deadValue;
    [SerializeField]
    private GameObjectEvent m_dynamicCameraAddEvent;
    [SerializeField]
    private GameObjectEvent m_dynamicCameraRemoveEvent;
    [SerializeField]
    private GameEvent m_dynamicCameraStartEvent;

    private bool m_punchedRight;
    private bool m_punchedLeft;
    private bool m_isPunching;

    private float m_knockbackScale;

    private InputMapper m_input;

    public bool m_leftPunch;

    private bool m_punchQueued;
    private bool m_inRing;

    private void Awake()
    {
    }

    private void Start()
    {
        m_dynamicCameraAddEvent.Invoke(gameObject);
        m_input = GetComponent<InputMapper>();
        m_dynamicCameraStartEvent.Invoke();
        foreach (Renderer rend in GetComponentsInChildren<Renderer>())
            rend.material.color = m_playerData.Character.TempColour;

        m_speedScale.Value = 1;
        m_knockbackScale = 1;
        //Instantiate(m_playerData.Character.PoultryBashCharacter, transform);
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
            m_dynamicCameraRemoveEvent.Invoke(gameObject);
        }
    }

    public void ScaleSpeed(float scale)
    {
        m_speedScale.Value = scale;
    }

    public void ScaleSpeedForSeconds(float scale, float seconds)
    {
        StopCoroutine("ResetSpeedScale");
        m_speedScale.Value = scale;
        StartCoroutine(ResetSpeedScale(seconds));
    }

    public void ScaleKnockbackForSeconds(float scale, float seconds)
    {
        StopCoroutine("ResetKnockbackScale");
        m_knockbackScale = scale;
        StartCoroutine(ResetKnockbackScale(seconds));
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

    public void Punch()
    {
        if (!m_inRing)
            return;
        RaycastHit hit;
        if (Physics.CapsuleCast((transform.position - transform.up / 2) - transform.forward * m_punchRadius, (transform.position + transform.up / 2) - transform.forward * m_punchRadius, m_punchRadius, transform.forward, out hit, m_punchDistance, 1<<10))
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
        m_dynamicCameraRemoveEvent.Invoke(gameObject);
    }
}
