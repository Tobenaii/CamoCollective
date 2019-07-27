using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerClimbPlayerController : MonoBehaviour
{
    [SerializeField]
    private float m_rotateSpeed;
    [SerializeField]
    private float m_climbSpeed;
    [SerializeField]
    private float m_fallSpeed;
    [SerializeField]
    private float m_strafeSpeed;
    [SerializeField]
    private float m_autoClimbMoveSpeed;
    private Quaternion m_leftClimbRot;
    private Quaternion m_rightClimbRot;
    private Quaternion m_targetRot;
    private bool m_climbLeft;
    private bool m_atTargetRot;
    public bool m_playerHasControl;
    public bool m_isDead;
    private bool m_stopMoving;
    private Rigidbody m_rb;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StopClimber"))
            m_stopMoving = true;
    }

    private void Awake()
    {
        m_leftClimbRot = Quaternion.Euler(0, 0, 20);
        m_rightClimbRot = Quaternion.Euler(0, 0, -20);
        m_atTargetRot = true;
        m_playerHasControl = false;
        m_rb = GetComponent<Rigidbody>();
        m_isDead = false;
    }
    public void OnDeath()
    {
        m_isDead = true;
    }

    public void GiveControl()
    {
        m_playerHasControl = true;
        GetComponent<InputMapper>().EnableInput();
    }

    public void TakeControl()
    {
        m_playerHasControl = false;
        GetComponent<InputMapper>().DisableInput();
    }

    public void MovePlayer(Vector2 joystick)
    {
        if (m_isDead)
            return;
        m_rb.MovePosition(new Vector3(transform.position.x + joystick.x * m_strafeSpeed * Time.deltaTime, transform.position.y, transform.position.z));
    }

    public void Climb()
    {
        if (!m_atTargetRot)
            return;
        m_climbLeft = !m_climbLeft;
        if (m_climbLeft)
            m_targetRot = m_leftClimbRot;
        else
            m_targetRot = m_rightClimbRot;
        m_atTargetRot = false;
    }

    private void Update()
    {
        if (m_stopMoving)
            return;
        if (!m_playerHasControl)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + m_autoClimbMoveSpeed * Time.deltaTime, transform.position.z);
            Climb();
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, m_targetRot, m_rotateSpeed * Time.deltaTime);
        if (transform.rotation == m_targetRot)
        {
            m_atTargetRot = true;
        }
        else if (m_playerHasControl)
            transform.position = new Vector3(transform.position.x, transform.position.y + m_climbSpeed * Time.deltaTime, transform.position.z);
        if (m_playerHasControl)
            transform.position = new Vector3(transform.position.x, transform.position.y - m_fallSpeed * Time.deltaTime, transform.position.z);
    }
}
