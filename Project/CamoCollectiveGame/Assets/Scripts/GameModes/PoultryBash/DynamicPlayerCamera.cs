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

    private List<GameObject> m_gameObjects = new List<GameObject>();

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

    public void AddGameObject(GameObject obj)
    {
        m_gameObjects.Add(obj);
    }

    public void RemoveGameObject(GameObject obj)
    {
        m_gameObjects.Remove(obj);
    }

    public void ClearGameObjects()
    {
        m_gameObjects.Clear();
    }

    private void LateUpdate()
    {
        if (m_gameObjects.Count == 0 || !m_cameraStarted)
            return;

        Bounds bounds = new Bounds(m_gameObjects[0].transform.position, Vector3.zero);
        foreach (GameObject obj in m_gameObjects)
            bounds.Encapsulate(obj.transform.position);

        m_midPos = bounds.center;

        float size = (bounds.size.x > bounds.size.z) ? bounds.size.x : bounds.size.z;
        size = Mathf.Clamp(size, m_minimumZoom, m_maximumZoom);
        transform.position = Vector3.SmoothDamp(transform.position, (m_midPos + m_offset) - transform.forward * size * m_zoomBufferSpace, ref m_zoomVelocity, m_zoomSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(m_midPos, Vector3.one);
    }
}
