using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChonkJoustingController : MonoBehaviour
{
    private Rigidbody m_rb;
    [SerializeField]
    private float m_chonkRunSpeed;
    [SerializeField]
    private float m_chonkBackwardVelocityMultiplier;
    [SerializeField]
    private float m_chonkAcceleration;
    [SerializeField]
    private float m_chonkMudAcceleration;
    [SerializeField]
    private float m_chonkStopSpeed;
    [SerializeField]
    private float m_chonkMudStopSpeedMultiplier;
    [SerializeField]
    private float m_chonkLookRotateSpeed;
    [SerializeField]
    private float m_chonkMoveRotateSpeed;
    [SerializeField]
    private GameEvent m_spawnedEvent;


    private Vector3 m_velocity;
    private Vector3 m_lookDir;
    private bool m_isSliding;
    private Vector3 m_smoothVelocity;
    private bool m_deathInvoked;

    private void OnTriggerEnter(Collider other)
    {
        if (m_deathInvoked)
            return;
        if (other.CompareTag("Balcony"))
        {
            GetComponent<InputMapper>().EnableInput();
            m_spawnedEvent.Invoke();
            m_deathInvoked = true;
        }
    }

    public Vector3 GetVelocity()
    {
        return m_velocity;
    }

    public void Respawn()
    {
        GetComponent<InputMapper>().DisableInput();
        m_velocity = transform.forward * m_chonkRunSpeed;
        m_deathInvoked = false;
    }

    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        QualitySettings.vSyncCount = 0;
    }

    private void FixedUpdate()
    {
        m_rb.MovePosition(transform.position + m_velocity * Time.deltaTime);
        if (m_lookDir == Vector3.zero)
            RotateChonkOverTime(m_velocity, m_chonkMoveRotateSpeed);
        else
            RotateChonkOverTime(m_lookDir, m_chonkLookRotateSpeed);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Mud"))
            m_isSliding = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Mud"))
            m_isSliding = false;
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

    private void RotateChonkInstant(Vector3 dir)
    {
        if (dir == Vector3.zero)
            return;
        transform.forward = dir;
    }

    public void Move(Vector2 joystick)
    {
        if (Vector3.Magnitude(joystick) < 0.3f)
        {
            m_velocity = Vector3.MoveTowards(m_velocity, Vector3.zero, m_chonkStopSpeed * (m_isSliding? m_chonkMudStopSpeedMultiplier : 1) * Time.deltaTime);
            return;
        }
        float backwardDot = Vector3.Dot(m_velocity, transform.forward);
        float backwardMultiplier = Mathf.InverseLerp(1, -1, backwardDot);
        float multiplier = Mathf.Lerp(1, m_chonkBackwardVelocityMultiplier, backwardMultiplier);
        Vector3 target = transform.forward + new Vector3(joystick.x, 0, joystick.y).normalized * (m_chonkRunSpeed * multiplier);
        if (m_isSliding)
            m_velocity = Vector3.MoveTowards(m_velocity, target, m_chonkMudAcceleration * Time.deltaTime);
        else
            m_velocity = Vector3.MoveTowards(m_velocity, target, m_chonkAcceleration * Time.deltaTime);
    }

    public void Look(Vector2 joystick)
    {
        m_lookDir = new Vector3(joystick.x, 0, joystick.y);
    }
}
