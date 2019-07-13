using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    [SerializeField]
    private float m_panSpeed;
    private GameObject m_currentTarget;
    private bool m_atTarget;
    private Vector3 m_velocity;
    private float m_panTime;

    private void Start()
    {
        m_atTarget = true;
    }

    private void Update()
    {
        if (m_atTarget)
            return;
        Vector3 targetPos = new Vector3(m_currentTarget.transform.position.x, transform.position.y, m_currentTarget.transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref m_velocity, 1 / m_panSpeed * m_panTime);
        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
            m_atTarget = true;
    }

    public void SetTarget(GameObject target)
    {
        m_currentTarget = target;
        m_atTarget = false;
        m_panTime = 1 / Vector3.Magnitude(transform.position - target.transform.position);
    }
}
