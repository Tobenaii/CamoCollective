using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicPlayerCamera : MonoBehaviour
{   
    [Header("Zoom Values")]
    [SerializeField]
    private float m_zoomBufferSpace;
    [SerializeField]
    private float m_zoomSpeed;
    [SerializeField]
    private float m_minimumZoom;
    [SerializeField]
    private float m_maximumZoom;
    [SerializeField]
    private Vector3 m_offset;
    [SerializeField]
    private AnimationCurve m_zoomCurve;

    [Header("Data")]
    [SerializeField]
    private GameObjectListSet m_gameObjectListSet;

    private Vector3 m_basePos;
    private float m_baseDist;
    private bool m_cameraStarted;
    private Vector3 m_midPos;

    private Vector3 m_zoomVelocity;

    public void StartDynamicCamera()
    {
        m_cameraStarted = true;
    }

    public void StopDynamicCamera()
    {
        m_cameraStarted = false;
    }

    private void FixedUpdate()
    {
        if (m_gameObjectListSet.Count == 0 || !m_cameraStarted)
            return;

        Bounds bounds = new Bounds(m_gameObjectListSet[0].transform.position, Vector3.zero);
        foreach (GameObject obj in m_gameObjectListSet.List)
            bounds.Encapsulate(obj.transform.position);

        m_midPos = bounds.center;

        float size = (bounds.size.x > bounds.size.z) ? bounds.size.x : bounds.size.z;
        size = Mathf.Clamp(size, m_minimumZoom, m_maximumZoom);
        float normSize = Mathf.InverseLerp(m_minimumZoom, m_maximumZoom, size);
        float newNormSize = m_zoomCurve.Evaluate(normSize);
        float newSize = Mathf.Lerp(m_minimumZoom, m_maximumZoom, newNormSize);
        transform.position = Vector3.SmoothDamp(transform.position, (m_midPos + m_offset) - transform.forward * newSize * m_zoomBufferSpace, ref m_zoomVelocity, m_zoomSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(m_midPos, Vector3.one);
    }
}
