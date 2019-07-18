using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField]
    private Vector3 m_targetPos;
    [SerializeField]
    private Vector3 m_targetRot;
    [SerializeField]
    private float m_moveSpeed;
    [SerializeField]
    private float m_rotationSpeed;
    [SerializeField]
    private GameEvent m_onCameraMoved;

    // Update is called once per frame
    void Update()
    {
        Camera.main.transform.rotation = Quaternion.RotateTowards(Camera.main.transform.rotation, Quaternion.Euler(m_targetRot), m_rotationSpeed * Time.deltaTime);
        Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, m_targetPos, m_moveSpeed * Time.deltaTime);
        if (Camera.main.transform.rotation == Quaternion.Euler(m_targetRot) && Vector3.Distance(Camera.main.transform.position, m_targetPos) < 0.005f)
        {
            m_onCameraMoved?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
