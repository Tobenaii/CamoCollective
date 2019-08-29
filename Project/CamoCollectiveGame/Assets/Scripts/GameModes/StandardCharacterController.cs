using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardCharacterController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float m_moveSpeed;
    [SerializeField]
    private FloatReference m_moveSpeedScale;
    [SerializeField]
    private float m_backwardVelocityMultiplier;
    [SerializeField]
    private float m_acceleration;
    [SerializeField]
    private float m_stopSpeed;
    [Header("Rotation")]
    [SerializeField]
    private float m_lookRotateSpeed;
    [SerializeField]
    private float m_moveRotateSpeed;
    [SerializeField]
    private float m_minRotationValue;
    [Header("Dash")]
    [SerializeField]
    private FloatReference m_dashForceScale;
    [SerializeField]
    private float m_minDashForce;
    [SerializeField]
    private float m_maxDashForce;
    [SerializeField]
    [Range(0, 1)]
    private float m_dashChargeSpeed;
    [SerializeField]
    private float m_dashCooldown;
    [Header("Sprint")]
    [SerializeField]
    private FloatReference m_stamina;
    [SerializeField]
    [Range(0,1)]
    private float m_staminaRegenSpeed;
    [SerializeField]
    [Range(0, 1)]
    private float m_staminaDecreaseSpeed;
    [SerializeField]
    private float m_sprintMoveSpeed;
    [Header("Input")]
    private float m_joystickDeadZone;

    private Vector3 m_velocity;
    private Vector3 m_lookDir;
    private Quaternion m_targetRot;
    private bool m_isDashing;
    private float m_dashCooldownTimer;
    private float m_curMoveSpeed;
    private bool m_isSprinting;

    private Rigidbody m_rb;
    private bool m_lookOverride;

    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_curMoveSpeed = m_moveSpeed;
    }

    private void RotateChonkOverTime(Vector3 dir, float speed)
    {
        if (dir == Vector3.zero)
            return;
        Quaternion prevRot = transform.rotation;
        transform.forward = dir;
        Quaternion newRot = transform.rotation;
        m_targetRot = newRot;
        transform.rotation = prevRot;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, m_targetRot, speed * Time.deltaTime);
    }

    private void Update()
    {
        if (m_isSprinting)
        {
            m_stamina.Value = Mathf.MoveTowards(m_stamina.Value, 0, m_staminaDecreaseSpeed * Time.deltaTime);
            if (m_stamina.Value <= 0)
                EndSprint();
        }
        else
            m_stamina.Value = Mathf.MoveTowards(m_stamina.Value, 1, m_staminaRegenSpeed * Time.deltaTime);

        if (m_isDashing)
        {
            m_dashForceScale.Value = Mathf.MoveTowards(m_dashForceScale.Value, 1, m_dashChargeSpeed * Time.deltaTime);
            if (m_dashForceScale.Value >= 1)
                EndDash();
        }
        else if (m_dashCooldownTimer > 0)
        {
            m_dashCooldownTimer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        m_velocity = Vector3.MoveTowards(m_velocity, Vector3.zero, m_stopSpeed * Time.deltaTime);
        if (!m_isDashing && !m_lookOverride)
        {
            RotateChonkOverTime(m_velocity, m_moveRotateSpeed);
            m_rb.MovePosition(transform.position + transform.forward * m_velocity.magnitude * Time.deltaTime);
        }
        else if (m_lookOverride)
        {
            RotateChonkOverTime(m_lookDir, m_lookRotateSpeed);
            m_rb.MovePosition(transform.position + m_velocity * Time.deltaTime);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        Vector3 col = collision.contacts[0].point;
        float dot = Vector3.Dot((col - transform.position).normalized, m_velocity.normalized);
        if (dot > 0.4f)
            m_velocity = Vector3.zero;
    }

    public void StartDash()
    {
        if (m_dashCooldownTimer > 0)
            return;
        m_isDashing = true;
        m_dashForceScale.Value = 0;
        m_lookDir = transform.forward;
    }

    public void EndDash()
    {
        if (!m_isDashing)
            return;
        m_isDashing = false;
        m_rb.AddForce(transform.forward * Mathf.Lerp(m_minDashForce, m_maxDashForce, m_dashForceScale.Value), ForceMode.Impulse);
        m_dashCooldownTimer = m_dashCooldown;
    }

    public void StartSprint()
    {
        m_isSprinting = true;
    }

    public void EndSprint()
    {
        m_isSprinting = false;
    }

    public void Move(Vector2 joystick)
    {
        float mag = joystick.magnitude;
        if (mag <= m_joystickDeadZone || mag == 0)
            return;
        if (m_isDashing)
            Look(joystick);

        m_curMoveSpeed = (m_isSprinting ? m_sprintMoveSpeed : m_moveSpeed) * m_moveSpeedScale.Value;

        float backwardDot = Vector3.Dot(m_velocity, transform.forward);
        float backwardMultiplier = Mathf.InverseLerp(1, -1, backwardDot);
        float multiplier = Mathf.Lerp(1, m_backwardVelocityMultiplier, backwardMultiplier);
        Vector3 target = new Vector3(joystick.x, 0, joystick.y).normalized * (m_curMoveSpeed);
        m_velocity = Vector3.MoveTowards(m_velocity, target, m_acceleration * Time.deltaTime);
    }

    public void Look(Vector2 joystick)
    {
        if (Vector3.Magnitude(joystick) <= m_joystickDeadZone)
        {
            m_lookDir = transform.forward;
            m_lookOverride = false;
            return;
        }
        m_lookOverride = true;
        m_lookDir = new Vector3(joystick.x, 0, joystick.y);
    }
}
