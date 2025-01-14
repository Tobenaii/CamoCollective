﻿using System.Collections;
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

    [Header("Sounds")]
    [SerializeField]
    private AudioSource m_onObstacleHitSound;

    [Header("Data")]
    [SerializeField]
    private FloatValue m_yPosValue;
    [SerializeField]
    private Animator[] m_animators;
    [SerializeField]
    private GameEvent m_playerStartedMovingEvent;

    private Quaternion m_leftClimbRot;
    private Quaternion m_rightClimbRot;
    private Quaternion m_targetRot;
    private bool m_playerHasControl;
    private bool m_playerFalling;
    private bool m_stopMoving;
    private bool m_playedHitSound;

    private Rigidbody m_rb;

    private Vector3 m_targetPos;
    private Vector3 m_initPos;

    private float m_currentClimbSpeed;
    private float m_targetClimbSpeed;
    private bool m_reachedMiddle;

    private void Awake()
    {
        m_leftClimbRot = Quaternion.Euler(20, 0, 0);
        m_rightClimbRot = Quaternion.Euler(-20, 0, 0);
        m_playerHasControl = false;
        m_rb = GetComponent<Rigidbody>();
        //TakeControl();
    }

    private void Start()
    {
        m_targetPos = transform.position;
        m_animators = GetComponentsInChildren<Animator>();
        m_currentClimbSpeed = m_baseClimbSpeed;
        m_initPos = transform.position;
    }

    public void ReachedMiddle()
    {
        m_reachedMiddle = true;
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
        if (Physics.Raycast(transform.position, newPos - transform.position, out hit, 0.5f))
        {
            if (hit.transform.CompareTag("Player") && !(Physics.Raycast(hit.transform.position, newPos - transform.position, 0.5f)))
            {
                transform.position = newPos;
                MoveOtherPlayer(joystick, hit.transform);
            }
        }
        else
            transform.position = newPos;
    }

    public void MoveOtherPlayer(Vector2 joystick, Transform t)
    {
        RaycastHit hit;
        Vector3 newPos = new Vector3(t.position.x, t.position.y, t.position.z - joystick.x * m_strafeSpeed * Time.deltaTime);
        if (Physics.Raycast(t.position, newPos - t.position, out hit, 0.5f))
        {
            if (hit.transform.CompareTag("Player") && !(Physics.Raycast(hit.transform.position, newPos - t.position, 0.5f)))
            {
                t.position = newPos;
                MoveOtherPlayer(joystick, hit.transform);
            }
        }
        else
            t.position = newPos;
    }

    private void LateUpdate()
    {
        if (!m_reachedMiddle)
        {
            float lowestY = 999999;
            for (int i = 0; i < m_yPosValue.Count; i++)
            {
                if (m_yPosValue.GetValue(i) == 0)
                    continue;
                if (m_yPosValue.GetValue(i) < lowestY)
                    lowestY = m_yPosValue.GetValue(i);
            }

            if (lowestY == transform.position.y)
            {
                m_fallSpeed.Value = m_currentClimbSpeed;
                if (transform.position.y > m_initPos.y + 5)
                    m_playerStartedMovingEvent.Invoke();
            }
        }
    }

    private void Update()
    {
        float speedCap = (m_reachedMiddle) ? m_fallSpeed.Value : 5;
        if (!Mathf.Approximately(m_currentClimbSpeed, m_targetClimbSpeed))
            m_currentClimbSpeed = Mathf.MoveTowards(m_currentClimbSpeed, m_targetClimbSpeed, m_climbAcceleration * Time.deltaTime);
        m_targetClimbSpeed = Mathf.MoveTowards(m_targetClimbSpeed, m_baseClimbSpeed, m_climbSpeedDecrease * Time.deltaTime);
    
        m_currentClimbSpeed = Mathf.Clamp(m_currentClimbSpeed, 0, speedCap + 3);
        m_targetClimbSpeed = Mathf.Clamp(m_targetClimbSpeed, 0, speedCap + 3);


        if (m_stopMoving)
            return;
        RaycastHit hit;
        bool hitUp = Physics.Raycast(transform.position, Vector3.up, out hit, 1.0f);
        if (hitUp && hit.collider.CompareTag("Mud"))
            hitUp = false;
        if (hitUp && hit.transform.CompareTag("StopClimber"))
        {
            m_stopMoving = true;
            foreach (Animator anim in m_animators)
                anim.SetFloat("ClimbScale", 0);
            return;
        }
        if (hitUp)
        {
            if (!m_playedHitSound)
            {
                m_playedHitSound = true;
                m_onObstacleHitSound.Play();
            }
            foreach (Animator anim in m_animators)
                anim.SetFloat("ClimbScale", 0);
            transform.position = new Vector3(transform.position.x, hit.point.y - 1.0f, transform.position.z);
        }

        hitUp = Physics.Raycast(transform.position, Vector3.up, out hit, 2.0f);
        if (!hitUp)
            m_playedHitSound = false;

        if (!m_playerHasControl)
            m_currentClimbSpeed = m_fallSpeed.Value;
        transform.position += Vector3.up * m_currentClimbSpeed * Time.deltaTime;
        if (m_playerFalling)
            transform.position += Vector3.down * m_fallSpeed.Value * Time.deltaTime;
        foreach (Animator anim in m_animators)
            anim.SetFloat("ClimbScale", m_currentClimbSpeed);
        m_yPosValue.Value = transform.position.y;
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
