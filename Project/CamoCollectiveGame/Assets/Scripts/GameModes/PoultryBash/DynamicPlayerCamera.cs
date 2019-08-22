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

    private List<GameObject> m_gameObjects = new List<GameObject>();

    private Vector3 m_basePos;
    private float m_baseDist;
    private bool m_cameraStarted;
    private Vector3 m_midPos;

    private Vector3 m_zoomVelocity;

    public void StartDynamicCamera()
    {
        m_cameraStarted = true;
        m_baseDist = GetHighestDistance();
        m_basePos = transform.position;
    }

    public void StopDynamicCamera()
    {
        m_cameraStarted = false;
    }

    public void AddGameObject(GameObject obj)
    {
        m_gameObjects.Add(obj);
    }

    public void ClearGameObjects()
    {
        m_gameObjects.Clear();
    }

    private float GetHighestDistance()
    {
        //Find the greatest distance between two players
        float highestDist = 0;
        foreach (GameObject t1 in m_gameObjects)
        {
            foreach (GameObject t2 in m_gameObjects)
            {
                if (t1 == t2)
                    continue;
                Vector3 dir = t2.transform.position - t1.transform.position;
                float dist = Vector3.Magnitude(dir);
                if (dist > highestDist)
                {
                    m_midPos = t1.transform.position + dir / 2;
                    highestDist = dist;
                }
            }
        }
        return highestDist;
    }

    private void Update()
    {
        if (m_gameObjects.Count == 0 || !m_cameraStarted)
            return;

        float highestDist = GetHighestDistance();
        float distOffset = m_baseDist - highestDist;

        transform.position = Vector3.SmoothDamp(transform.position, m_midPos - (transform.forward * highestDist) * m_zoomBufferSpace, ref m_zoomVelocity, m_zoomSpeed);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(m_midPos - transform.position), 10 * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(m_midPos, Vector3.one);
    }
}
