using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardCharacterController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float m_moveSpeed;
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
    private float m_minDashForce;
    [SerializeField]
    private float m_maxDashForce;
    [SerializeField]
    private float m_dashChargeSpeed;
    [SerializeField]
    private float m_dashCooldown;
    [Header("Input")]
    private float m_joystickDeadZone;

    private Vector3 m_velocity;
    private Vector3 m_lookDir;
    private Quaternion m_targetRot;
    private float m_dashForce;
    private bool m_isDashing;
    private float m_dashCooldownTimer;

    private Rigidbody m_rb;

    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
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
        if (m_isDashing)
        {
            m_dashForce = Mathf.MoveTowards(m_dashForce, m_maxDashForce, m_dashChargeSpeed * Time.deltaTime);
            if (m_dashForce >= m_maxDashForce)
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
        if (!m_isDashing)
        {
            RotateChonkOverTime(m_velocity, m_moveRotateSpeed);
            m_rb.MovePosition(transform.position + transform.forward * m_velocity.magnitude * Time.deltaTime);
        }
        else
            RotateChonkOverTime(m_lookDir, m_lookRotateSpeed);
    }

    public void StartDash()
    {
        if (m_dashCooldownTimer > 0)
            return;
        m_isDashing = true;
        m_dashForce = m_minDashForce;
        m_lookDir = transform.forward;
    }

    public void EndDash()
    {
        if (!m_isDashing)
            return;
        m_isDashing = false;
        m_rb.AddForce(transform.forward * m_dashForce, ForceMode.Impulse);
        m_dashCooldownTimer = m_dashCooldown;
    }

    public void Move(Vector2 joystick)
    {
        float mag = joystick.magnitude;
        if (mag <= m_joystickDeadZone || mag == 0)
            return;
        if (m_isDashing)
            Look(joystick);

        float backwardDot = Vector3.Dot(m_velocity, transform.forward);
        float backwardMultiplier = Mathf.InverseLerp(1, -1, backwardDot);
        float multiplier = Mathf.Lerp(1, m_backwardVelocityMultiplier, backwardMultiplier);
        Vector3 target = new Vector3(joystick.x, 0, joystick.y).normalized * (m_moveSpeed * multiplier);
        m_velocity = Vector3.MoveTowards(m_velocity, target, m_acceleration * Time.deltaTime);
    }

    public void Look(Vector2 joystick)
    {
        if (Vector3.Magnitude(joystick) < m_joystickDeadZone)
        {
            m_lookDir = transform.forward;
            return;
        }
        m_lookDir = new Vector3(joystick.x, 0, joystick.y);
    }

    private void OnCollisionStay(Collision collision)
    {
        //Vector3 normal = collision.GetContact(0).normal;
        //if (normal.x > 0 && m_velocity.x < 0)
        //    m_velocity.x = 0;
        //if (normal.x < 0 && m_velocity.x > 0)
        //    m_velocity.x = 0;
        //if (normal.z < 0 && m_velocity.z > 0)
        //    m_velocity.z = 0;
        //if (normal.z > 0 && m_velocity.z < 0)
        //    m_velocity.z = 0;
    }
}
