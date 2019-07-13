using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChonkController : MonoBehaviour
{
    private Rigidbody m_rb;
    [SerializeField]
    private float m_chonkRunSpeed;
    [SerializeField]
    private float m_chonkStopSpeed;
    [SerializeField]
    private float m_chonkRotateSpeed;
    [SerializeField]
    private float m_joustRotateSpeed;
    [SerializeField]
    private float m_maxJoustAngle;
    [SerializeField]
    private GameObject m_joust;

    private Vector3 m_velocity;
    private Vector3 m_targetAim;

    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        QualitySettings.vSyncCount = 0;
    }

    private void FixedUpdate()
    {
        m_rb.MovePosition(transform.position + transform.forward * Vector3.Magnitude(m_velocity) * Time.deltaTime);
        RotateChonkOverTime();
        RotateJoustOverTime();
    }

    private void RotateChonkOverTime()
    {
        if (m_velocity == Vector3.zero)
            return;
        Quaternion prevRot = transform.rotation;
        transform.forward = m_velocity;
        Quaternion newRot = transform.rotation;
        transform.rotation = prevRot;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRot, m_chonkRotateSpeed);
    }

    private void RotateJoustOverTime()
    {
        Vector3 maxNegRot = Quaternion.AngleAxis(-m_maxJoustAngle, Vector3.up) * transform.forward;
        Vector3 maxPosRot = Quaternion.AngleAxis(m_maxJoustAngle, Vector3.up) * transform.forward;

        if (m_targetAim == Vector3.zero)
            return;
        Quaternion prevRot = m_joust.transform.rotation;
        m_joust.transform.forward = m_targetAim;
        Quaternion newRot = m_joust.transform.rotation;
        m_joust.transform.rotation = prevRot;
        float targetDot = Vector3.Dot(m_targetAim, transform.right);
        float frwdDot = Vector3.Dot(m_joust.transform.forward, transform.right);
        float angleNormal = Mathf.InverseLerp(90, 0, m_maxJoustAngle);
        if (targetDot < angleNormal && frwdDot < angleNormal)
            return;
        m_joust.transform.rotation = Quaternion.RotateTowards(m_joust.transform.rotation, newRot, m_joustRotateSpeed);
    }

    public void Move(Vector2 joystick)
    {
        if (Vector3.Magnitude(joystick) < 0.3f)
        {
            m_velocity = Vector3.MoveTowards(m_velocity, Vector3.zero, m_chonkStopSpeed * Time.deltaTime);
            return;
        }
        Vector3 frwd = new Vector3(joystick.x, 0, joystick.y);
        m_velocity = new Vector3(joystick.x, 0, joystick.y).normalized * m_chonkRunSpeed;
    }

    public void Aim(Vector2 joystick)
    {
        m_targetAim = new Vector3(joystick.x, 0, joystick.y);
        m_targetAim = Quaternion.AngleAxis(90, Vector3.up) * m_targetAim;
    }
}
