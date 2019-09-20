using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerClimbPlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float m_baseClimbSpeed;
    [SerializeField]
    private float m_climbSpeedIncrease;
    [SerializeField]
    private float m_climbSpeedDecrease;
    [SerializeField]
    private float m_maxClimbSpeed;
    [SerializeField]
    private float m_climbAcceleration;
    [SerializeField]
    private FloatReference m_fallSpeed;
    [SerializeField]
    private float m_strafeSpeed;

    [Header("Sticky Obstacle")]
    [SerializeField]
    private float m_stickyMovementScale;

    [Header("Data")]
    [SerializeField]
    private FloatReference m_yPosValue;
    [SerializeField]
    private Animator m_animator;

    private Quaternion m_leftClimbRot;
    private Quaternion m_rightClimbRot;
    private Quaternion m_targetRot;
    private bool m_playerHasControl;
    private bool m_playerFalling;
    private bool m_stopMoving;

    private Rigidbody m_rb;

    private Vector3 m_targetPos;

    private float m_currentClimbSpeed;
    private float m_targetClimbSpeed;

    private void Awake()
    {
        m_leftClimbRot = Quaternion.Euler(20, 0, 0);
        m_rightClimbRot = Quaternion.Euler(-20, 0, 0);
        m_playerHasControl = false;
        m_rb = GetComponent<Rigidbody>();
        TakeControl();
    }

    private void Start()
    {
        m_targetPos = transform.position;
        m_animator = GetComponentInChildren<Animator>();
        m_currentClimbSpeed = m_baseClimbSpeed;
    }

    public void ClimbFaster()
    {
        m_targetClimbSpeed += m_climbSpeedIncrease;
    }

    public void GiveControl()
    {
        m_playerHasControl = true;
        GetComponent<InputMapper>().EnableInput();
    }

    public void StartFalling()
    {
        m_playerFalling = true;
    }

    public void StopFalling()
    {
        m_playerFalling = false;
    }

    public void TakeControl()
    {
        m_playerHasControl = false;
        GetComponent<InputMapper>().DisableInput();
    }

    public void MovePlayer(Vector2 joystick)
    {
        RaycastHit hit;
        Vector3 newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - joystick.x * m_strafeSpeed * Time.deltaTime);
        if (!Physics.Raycast(transform.position, newPos - transform.position, out hit, 0.5f))
            transform.position = newPos;
    }

    private void Update()
    {
        if (!Mathf.Approximately(m_currentClimbSpeed, m_targetClimbSpeed))
            m_currentClimbSpeed = Mathf.MoveTowards(m_currentClimbSpeed, m_targetClimbSpeed, m_climbAcceleration * Time.deltaTime);
        m_targetClimbSpeed = Mathf.MoveTowards(m_targetClimbSpeed, m_baseClimbSpeed, m_climbSpeedDecrease * Time.deltaTime);

        m_currentClimbSpeed = Mathf.Clamp(m_currentClimbSpeed, 0, m_fallSpeed.Value + 3);
        m_targetClimbSpeed = Mathf.Clamp(m_targetClimbSpeed, 0, m_fallSpeed.Value + 3);

        Debug.Log("Current Speed: " + m_currentClimbSpeed);
        Debug.Log("Target Speed: " + m_targetClimbSpeed);

        m_animator.SetFloat("ClimbScale", m_currentClimbSpeed);
        m_yPosValue.Value = transform.position.y;
        if (m_stopMoving)
            return;
        RaycastHit hit;
        bool hitUp = Physics.Raycast(transform.position, Vector3.up, out hit, 1.0f);
        if (hitUp && hit.collider.CompareTag("Mud"))
            hitUp = false;
        if (hitUp && hit.transform.CompareTag("StopClimber"))
        {
            m_stopMoving = true;
            return;
        }
        if (hitUp)
            transform.position = new Vector3(transform.position.x, hit.point.y - 1.0f, transform.position.z);
        if (m_playerHasControl)
            transform.position += Vector3.up * m_currentClimbSpeed * Time.deltaTime;
        else
            transform.position += Vector3.up * m_fallSpeed.Value * Time.deltaTime;

        if (m_playerFalling)
            transform.position += Vector3.down * m_fallSpeed.Value * Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Mud"))
            m_currentClimbSpeed = Mathf.MoveTowards(m_currentClimbSpeed, m_baseClimbSpeed, m_climbSpeedDecrease * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {

    }
}
