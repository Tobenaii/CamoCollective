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
    [Header("Input")]
    private float m_joystickDeadZone;

    private Vector3 m_velocity;
    private Vector3 m_lookDir;

    private Rigidbody m_rb;

    private Queue<Quaternion> m_rotationQueue = new Queue<Quaternion>();
    private Quaternion m_lastRotAdded;

    // Start is called before the first frame update
    void Start()
    {
        m_lookDir = transform.forward;
        m_rb = GetComponent<Rigidbody>();
    }

    private void RotateChonkOverTimeNew(Vector3 dir, float speed)
    {
        if (dir == Vector3.zero)
        {
            m_rotationQueue.Clear();
            return;
        }

        Quaternion prevRot = transform.rotation;
        transform.forward = dir;
        Quaternion newRot = transform.rotation;
        transform.rotation = prevRot;

        if (m_rotationQueue.Count == 0 || newRot != m_lastRotAdded)
        {
            if (Quaternion.Angle(newRot, transform.rotation) > m_minRotationValue)
            {
                m_rotationQueue.Enqueue(newRot);
                m_lastRotAdded = newRot;
            }
        }
        else
        {
            m_rotationQueue.Clear();
            m_rotationQueue.Enqueue(newRot);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, (m_rotationQueue.Count > 0) ? m_rotationQueue.Peek() : transform.rotation, speed);
        if (m_rotationQueue.Count > 0 && transform.rotation == m_rotationQueue.Peek())
        {
            m_rotationQueue.Dequeue();
        }
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
        m_velocity = Vector3.MoveTowards(m_velocity, target, m_acceleration * Time.deltaTime);
    }

    public void Look(Vector2 joystick)
    {
        Move(joystick);
        if (Vector3.Magnitude(joystick) < m_joystickDeadZone)
        {
            m_lookDir = Vector3.zero;
            return;
        }
        m_lookDir = new Vector3(joystick.x, 0, joystick.y);
    }

    private void OnCollisionStay(Collision collision)
    {
        Vector3 normal = collision.GetContact(0).normal;
        if (normal.x > 0 && m_velocity.x < 0)
            m_velocity.x = 0;
        if (normal.x < 0 && m_velocity.x > 0)
            m_velocity.x = 0;
        if (normal.z < 0 && m_velocity.z > 0)
            m_velocity.z = 0;
        if (normal.z > 0 && m_velocity.z < 0)
            m_velocity.z = 0;
    }
}
