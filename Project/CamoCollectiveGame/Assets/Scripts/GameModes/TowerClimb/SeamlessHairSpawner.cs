using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeamlessHairSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObjectPool m_hairPool;
    [SerializeField]
    private BoolValue m_moveValue;
    [SerializeField]
    private int m_initAmmount;
    [SerializeField]
    private GameEvent m_stopMovingHairEvent;
    [SerializeField]
    private float m_offset;
    private float m_height;
    private GameObject m_prevHair;
    private bool m_queueStopSpawn;
    private bool m_stopSpawn;


    private void OnEnable()
    {
        GameObject hairPeek = m_hairPool.PeekObject();
        hairPeek.transform.localScale = new Vector3(2, 2, 2);
        m_height = hairPeek.GetComponentInChildren<Renderer>().bounds.size.y;
        for (int i = 0; i < m_initAmmount; i++)
        {
            GameObject hair = m_hairPool.GetObject();
            hair.transform.position = new Vector3(transform.position.x, transform.position.y - (m_height - m_offset) * (i), transform.position.z);
            hair.transform.rotation = transform.rotation;
            hair.transform.SetParent(transform);
            hair.transform.localScale = new Vector3(2, 2, 2);
            if (i == 0)
                m_prevHair = hair;
        }
    }

    public void StartMoving()
    {
        m_moveValue.Value = true;
    }

    public void StopMoving()
    {
        m_queueStopSpawn = true;
    }

    private void LateUpdate()
    {
        if (m_prevHair == null)
            return;
        if (m_prevHair.transform.position.y < transform.position.y - m_height)
        {
            GameObject hair = m_hairPool.GetObject();
            hair.transform.position = new Vector3(transform.position.x, m_prevHair.transform.position.y + (m_height - m_offset), transform.position.z);
            hair.transform.rotation = transform.rotation;
            hair.transform.SetParent(transform);
            hair.transform.localScale = new Vector3(2, 2, 2);
            m_prevHair = hair;
            if (m_queueStopSpawn)
            {
                hair = m_hairPool.GetObject();
                hair.transform.position = new Vector3(transform.position.x, m_prevHair.transform.position.y + (m_height - m_offset), transform.position.z);
                hair.transform.rotation = transform.rotation;
                hair.transform.SetParent(transform);
                hair.transform.localScale = new Vector3(2, 2, 2);
                m_prevHair = hair;
                m_stopSpawn = true;
                m_moveValue.Value = false;
            }
        }
    }
}
