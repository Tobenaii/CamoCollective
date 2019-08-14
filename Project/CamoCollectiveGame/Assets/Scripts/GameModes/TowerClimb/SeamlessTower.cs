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

    private Renderer m_renderer;
    private GameObject m_topMiddleTower;

    private bool m_move;

    private Vector3 m_returnPoint;

    private void Start()
    {
        m_renderer = m_middleTower.GetComponent<Renderer>();
        m_middleTower2 = Instantiate(m_middleTower, m_middleTower.transform.position + Vector3.up * m_renderer.bounds.size.y, m_middleTower.transform.rotation, m_middleTower.transform.parent);
        m_topTower.transform.position = m_middleTower2.transform.position;
        m_returnPoint = m_middleTower.transform.position;
        m_topMiddleTower = m_middleTower2;
    }

    //public void StartMoving()
    //{
    //    m_move = true;
    //}

    //public void StopMoving()
    //{
    //    m_topTower.SetActive(true);
    //    m_topTower.transform.position = m_topMiddleTower.transform.position;
    //    m_move = false;
    //}

    private void Update()
    {
        //if (!m_move)
        //    return;
        //if (m_middleTower.transform.position.y <= m_returnPoint.y)
        //{
        //    m_middleTower.transform.position = m_middleTower2.transform.position + Vector3.up * m_renderer.bounds.size.y;
        //    m_topMiddleTower = m_middleTower;
        //}
        //if (m_middleTower2.transform.position.y <= m_returnPoint.y)
        //{
        //    m_middleTower2.transform.position = m_middleTower.transform.position + Vector3.up * m_renderer.bounds.size.y;
        //    m_topMiddleTower = m_middleTower2;
        //}

        //m_middleTower.transform.position += Vector3.down * m_moveSpeed.Value * Time.deltaTime;
        //m_middleTower2.transform.position += Vector3.down * m_moveSpeed.Value * Time.deltaTime;
    }
}
