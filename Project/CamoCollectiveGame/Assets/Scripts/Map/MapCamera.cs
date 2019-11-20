using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    [SerializeField]
    private float m_panTime;
    [SerializeField]
    private float m_rotateTime;
    [SerializeField]
    private float m_bezierTime;
    [SerializeField]
    private GameEvent m_cameraReachedTargetEvent;
    private Vector3 m_currentTargetPos;
    private Quaternion m_currentTargetRot;
    private bool m_atTarget;
    private Vector3 m_velocity;
    private Vector3 m_targetRot;

    private Vector3 m_initPos;
    private TimeLerper m_lerper = new TimeLerper();
    private Vector3 m_prevPos;

    private float m_rotateVelocityX;
    private float m_rotateVelocityY;
    private float m_rotateVelocityZ;

    private bool m_triggeredEvent;

    private void Awake()
    {
        GetComponent<Camera>().depth = -1;
    }

    private void Start()
    {
        m_atTarget = true;
    }

    private void Update()
    {
        if (m_atTarget)
            return;
        m_prevPos = transform.position;
        Vector3 dir = m_currentTargetPos - m_initPos;
        Vector3 perp = new Vector3(-dir.z, dir.y, dir.x);
        Vector3 targetPos = m_lerper.BezierCurve(m_initPos, (m_initPos + dir / 2) + perp / 2, m_currentTargetPos, m_bezierTime);

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref m_velocity, m_panTime);

        Quaternion targetRot;
        if (Vector3.Distance(transform.position, targetPos) < 10.0f)
            targetRot = Quaternion.LookRotation(transform.position - m_prevPos, Vector3.up);
        else
            targetRot = m_currentTargetRot;

        Quaternion prevRot = transform.rotation;
        transform.forward = targetPos - transform.position;
        Quaternion newRot = transform.rotation;
        transform.rotation = prevRot;
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, m_rotateTime * Time.deltaTime);
        float xAngle = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.x, m_currentTargetRot.eulerAngles.x,
                                                                                ref m_rotateVelocityX, m_rotateTime);
        float yAngle = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, m_currentTargetRot.eulerAngles.y,
                                                                                ref m_rotateVelocityY, m_rotateTime);
        float zAngle = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.z, m_currentTargetRot.eulerAngles.z,
                                                                                ref m_rotateVelocityZ, m_rotateTime);

        transform.rotation = Quaternion.Euler(new Vector3(xAngle, yAngle, zAngle));

        if (!m_triggeredEvent && Vector3.Distance(transform.position, m_currentTargetPos) < 0.5f)
        {
            m_triggeredEvent = true;
            m_cameraReachedTargetEvent.Invoke();
        }

        if (Vector3.Distance(transform.position, m_currentTargetPos) < 0.1f && transform.rotation == m_currentTargetRot)
            m_atTarget = true;
    }

    public void SetTarget(GameObject target)
    {
        if (Vector3.Distance(target.transform.position, transform.position) < 5f && Quaternion.Angle(target.transform.rotation, transform.rotation) < 5f)
            return;
        if (target != null)
        {
            m_currentTargetPos = target.transform.position;
            m_currentTargetRot = target.transform.rotation;
            m_atTarget = false;
        }
        else
        {
            m_atTarget = true;
        }
        m_triggeredEvent = false;
        m_initPos = transform.position;
        m_lerper.Reset();
    }
}
