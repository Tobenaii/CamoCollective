using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    [SerializeField]
    private float m_panTime;
    [SerializeField]
    private float m_rotateTime;
    private GameObject m_currentTarget;
    private bool m_atTarget;
    private Vector3 m_velocity;
    private Vector3 m_rotateVelocity;
    private Vector3 m_targetRot;

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
        Vector3 targetPos = m_currentTarget.transform.position;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref m_velocity, m_panTime);

        transform.rotation = Quaternion.Slerp(transform.rotation, m_currentTarget.transform.rotation, m_rotateTime * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.01f && transform.rotation == m_currentTarget.transform.rotation)
            m_atTarget = true;
    }

    public void SetTarget(GameObject target)
    {
        m_currentTarget = target;
        m_atTarget = false;
    }
}
