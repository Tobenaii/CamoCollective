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
    [Header("Input")]
    private float m_joystickDeadZone;

    private Vector3 m_velocity;
    private Vector3 m_lookDir;

    private Rigidbody m_rb;

    // Start is called before the first frame update
    void Start()
    {
        m_lookDir = transform.forward;
        m_rb = GetComponent<Rigidbody>();
    }

    private void RotateChonkOverTime(Vector3 dir, float speed)
    {
        if (dir == Vector3.zero)
            return;
        Quaternion prevRot = transform.rotation;
        transform.forward = dir;
        Quaternion newRot = transform.rotation;
        transform.rotation = prevRot;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRot, speed);
    }

    private void FixedUpdate()
    {
        m_rb.MovePosition(transform.position + m_velocity * Time.deltaTime);
        if (m_lookDir == Vector3.zero)
            RotateChonkOverTime(m_velocity, m_moveRotateSpeed);
        else
            RotateChonkOverTime(m_lookDir, m_lookRotateSpeed);
    }

    public void Move(Vector2 joystick)
    {
        float mag = joystick.magnitude;
        if (mag <= m_joystickDeadZone || mag == 0)
        {
            m_velocity = Vector3.MoveTowards(m_velocity, Vector3.zero, m_stopSpeed * Time.deltaTime);
            return;
        }

        float backwardDot = Vector3.Dot(m_velocity, transform.forward);
        float backwardMultiplier = Mathf.InverseLerp(1, -1, backwardDot);
        float multiplier = Mathf.Lerp(1, m_backwardVelocityMultiplier, backwardMultiplier);
        Vector3 target = new Vector3(joystick.x, 0, joystick.y).normalized * (m_moveSpeed * multiplier);
        if (m_velocity.sqrMagnitude > target.sqrMagnitude)
            return;
        m_velocity = Vector3.MoveTowards(m_velocity, target, m_acceleration * Time.deltaTime);

    }

    public void Look(Vector2 joystick)
    {
        if (Vector3.Magnitude(joystick) < m_joystickDeadZone)
            return;
        m_lookDir = new Vector3(joystick.x, 0, joystick.y);
    }
}
