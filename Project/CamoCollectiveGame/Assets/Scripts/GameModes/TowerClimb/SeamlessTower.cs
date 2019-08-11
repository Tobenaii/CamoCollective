using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeamlessTower : MonoBehaviour
{
    [SerializeField]
    private GameObject m_middleTower;
    private GameObject m_middleTower2;

    [SerializeField]
    private GameObject m_topTower;
    [SerializeField]
    private FloatValue m_moveSpeed;
    [SerializeField]
    private GameEvent m_stopMovingEvent;

    private Renderer m_renderer;

    private bool m_move;
    private bool m_queuedStop;

    private Vector3 m_returnPoint;

    private void Start()
    {
        m_renderer = m_middleTower.GetComponent<Renderer>();
        m_middleTower2 = Instantiate(m_middleTower, m_middleTower.transform.position + Vector3.up * m_renderer.bounds.size.y, m_middleTower.transform.rotation, m_middleTower.transform.parent);
        GameObject m_middleTower3 = Instantiate(m_middleTower2, m_middleTower2.transform.position + Vector3.up * m_renderer.bounds.size.y, m_middleTower2.transform.rotation, m_middleTower2.transform.parent);
        m_topTower.transform.position = m_middleTower3.transform.position;

        m_returnPoint = m_middleTower.transform.position;
    }

    public void StartMoving()
    {
        m_move = true;
    }

    public void StopMoving()
    {
        m_queuedStop = true;
    }

    private bool CheckStop()
    {
        if (m_queuedStop)
        {
            m_move = false;
            m_queuedStop = false;
            m_stopMovingEvent.Invoke();
            return true;
        }
        return false;
    }

    private void Update()
    {
        if (!m_move)
        {
            CheckStop();
            return;
        }
        if (m_middleTower.transform.position.y <= m_returnPoint.y)
        {
            if (CheckStop())
                return;
            m_middleTower.transform.position = m_middleTower2.transform.position + Vector3.up * m_renderer.bounds.size.y;
        }
        if (m_middleTower2.transform.position.y <= m_returnPoint.y)
        {
            if (CheckStop())
                return;
            m_middleTower2.transform.position = m_middleTower.transform.position + Vector3.up * m_renderer.bounds.size.y;
        }

        m_middleTower.transform.position += Vector3.down * m_moveSpeed.Value * Time.deltaTime;
        m_middleTower2.transform.position += Vector3.down * m_moveSpeed.Value * Time.deltaTime;
    }
}
