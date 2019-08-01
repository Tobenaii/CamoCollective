using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField]
    private bool m_move = true;
    [SerializeField]
    private bool m_rotate = true;
    [SerializeField]
    private Vector3 m_targetPos;
    [SerializeField]
    private Vector3 m_targetRot;
    [SerializeField]
    private bool m_snap;
    [SerializeField]
    private bool m_smooth;
    [SerializeField]
    private float m_moveSpeed;
    [SerializeField]
    private FloatValue m_scale;
    [SerializeField]
    private bool m_timeInstead;
    [SerializeField]
    private float m_rotationSpeed;
    [SerializeField]
    private GameEvent m_onCameraMoved;

    private Vector3 m_rotateVelocty;
    private Vector3 m_moveVelocity;
    private TimeLerper m_lerper;
    private Vector3 m_initPos;

    private void Awake()
    {
        m_lerper = new TimeLerper();
        m_initPos = Camera.main.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float speed = m_moveSpeed * (m_scale == null ? 1 : m_scale.value);
        if (m_snap)
        {
            if (m_rotate)
                Camera.main.transform.rotation = Quaternion.Euler(m_targetRot);
            if (m_move)
                Camera.main.transform.position = m_targetPos;
            m_onCameraMoved?.Invoke();
            gameObject.SetActive(false);
            return;
        }
        if (m_rotate)
        {
            if (!m_smooth)
                Camera.main.transform.rotation = Quaternion.RotateTowards(Camera.main.transform.rotation, Quaternion.Euler(m_targetRot), m_rotationSpeed * Time.deltaTime);
            else
                Camera.main.transform.rotation = Quaternion.Euler(Vector3.SmoothDamp(Camera.main.transform.rotation.eulerAngles, m_targetRot, ref m_rotateVelocty, m_rotationSpeed));
        }
        if (m_move)
        {
            if (!m_smooth)
                Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, m_targetPos, speed * Time.deltaTime);
            else if (m_timeInstead)
                Camera.main.transform.position = m_lerper.Lerp(m_initPos, m_targetPos, speed);
            else
                Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, m_targetPos, ref m_moveVelocity, speed);
        }
        if ((Vector3.Distance(Camera.main.transform.rotation.eulerAngles, m_targetRot) < 0.05f || !m_rotate) && (Vector3.Distance(Camera.main.transform.position, m_targetPos) < 0.05f || !m_move))
        {
            m_onCameraMoved?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
