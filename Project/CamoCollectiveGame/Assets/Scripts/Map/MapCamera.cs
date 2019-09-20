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
    private GameObject m_currentTarget;
    private bool m_atTarget;
    private Vector3 m_velocity;
    private Vector3 m_rotateVelocity;
    private Vector3 m_targetRot;

    private Vector3 m_initPos;
    private TimeLerper m_lerper = new TimeLerper();
    private Vector3 m_prevPos;

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
        if (m_atTarget || m_currentTarget == null)
            return;
        m_prevPos = transform.position;
        Vector3 dir = m_currentTarget.transform.position - m_initPos;
        Vector3 perp = new Vector3(-dir.z, dir.y, dir.x);
        Vector3 targetPos = m_lerper.BezierCurve(m_initPos, (m_initPos + dir / 2) + perp / 2, m_currentTarget.transform.position, m_bezierTime);

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref m_velocity, m_panTime);

        Quaternion targetRot;
        if (Vector3.Distance(transform.position, targetPos) < 10.0f)
            targetRot = Quaternion.LookRotation(transform.position - m_prevPos, Vector3.up);
        else
            targetRot = m_currentTarget.transform.rotation;

        Quaternion prevRot = transform.rotation;
        transform.forward = targetPos - transform.position;
        Quaternion newRot = transform.rotation;
        transform.rotation = prevRot;
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, m_rotateTime * Time.deltaTime);
        transform.rotation = Quaternion.Euler(Vector3.SmoothDamp(transform.rotation.eulerAngles, m_currentTarget.transform.rotation.eulerAngles,
                                                                                ref m_rotateVelocity, m_panTime));

        if (Vector3.Distance(transform.position, targetPos) < 0.1f && transform.rotation == m_currentTarget.transform.rotation)
        {
            transform.position = targetPos;
            m_cameraReachedTargetEvent.Invoke();
            m_atTarget = true;
        }
    }

    public void SetTarget(GameObject target)
    {
        m_currentTarget = target;
        m_atTarget = false;
        m_initPos = transform.position;
        m_lerper.Reset();
    }
}
