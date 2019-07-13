using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField]
    private int m_elevationLevels;
    [SerializeField]
    private float m_initHeight;
    [SerializeField]
    private float m_minHeightFromGround;
    [SerializeField]
    private float m_zoomSpeed;
    [SerializeField]
    private float m_rotationSpeed;
    [SerializeField]
    private AnimationCurve m_rotationCurve;


    private Vector3 m_velocity;
    private float m_rotationVelocity;

    private float m_currentLevel;
    private Vector3 m_target;
    private float m_targetRot;
    private float m_initRot;
    private float m_finalRot;

    private void Start()
    {
        m_currentLevel = m_elevationLevels;
        m_initRot = 90;
        m_finalRot = 0;
    }

    private void Update()
    {
        m_currentLevel += Input.mouseScrollDelta.y;
        m_currentLevel = Mathf.Clamp(m_currentLevel, 0, m_elevationLevels);
        float normalLevel = Mathf.InverseLerp(m_elevationLevels, 0, m_currentLevel);
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(transform.position.x, m_initHeight, transform.position.z), Vector3.down, out hit))
        {
            m_target = new Vector3(transform.position.x, m_initHeight - (hit.distance - m_minHeightFromGround) * normalLevel, transform.position.z);
        }
        transform.position = Vector3.SmoothDamp(transform.position, m_target, ref m_velocity, m_zoomSpeed * Time.deltaTime);
        float rotLevel = m_rotationCurve.Evaluate(normalLevel);
        m_targetRot = Mathf.Lerp(m_initRot, m_finalRot, rotLevel);
        transform.rotation = Quaternion.Euler(Mathf.SmoothDamp(transform.rotation.eulerAngles.x, m_targetRot, ref m_rotationVelocity, m_rotationSpeed * Time.deltaTime), 0, 0);
    }

}