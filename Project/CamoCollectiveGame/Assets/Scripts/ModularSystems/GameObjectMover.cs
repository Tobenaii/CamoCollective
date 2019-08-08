using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectMover : MonoBehaviour
{
    [SerializeField]
    private Vector3 m_direction;
    [SerializeField]
    private float m_moveSpeed;
    [SerializeField]
    private FloatValue m_scale;
    [SerializeField]
    private bool m_moveOnAwake;
    private bool m_move;

    private void Awake()
    {
        m_move = m_moveOnAwake;
    }

    public void StartMoving()
    {
        m_move = true;
    }

    public void StopMoving()
    {
        m_move = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_move)
            return;
        transform.position += m_direction.normalized * m_moveSpeed * m_scale.Value * Time.deltaTime;
    }
}
