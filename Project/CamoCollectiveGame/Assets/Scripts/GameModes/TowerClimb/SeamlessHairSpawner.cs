using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeamlessHairSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject m_hairPrefab;
    [SerializeField]
    private int m_initAmmount;
    [SerializeField]
    private GameEvent m_stopMovingHairEvent;
    private float m_height;
    private List<GameObject> m_hair = new List<GameObject>();
    private bool m_queueStopSpawn;
    private bool m_stopSpawn;

    private void OnEnable()
    {
        m_height = m_hairPrefab.GetComponent<Renderer>().bounds.size.y;
        for (int i = 0; i < m_initAmmount; i++)
        {
            GameObject hair = Instantiate(m_hairPrefab, new Vector3(transform.position.x, transform.position.y - (m_height) * (i), transform.position.z), transform.rotation, transform);
            m_hair.Add(hair);
        }
    }

    public void StopSpawningHair()
    {
        m_queueStopSpawn = true;
    }

    private void LateUpdate()
    {
        if (m_hair.Count == 0 || m_hair[0] == null || m_stopSpawn)
            return;
        if (m_hair[0].transform.position.y < transform.position.y - m_height)
        {
            GameObject hair = Instantiate(m_hairPrefab, new Vector3(transform.position.x, m_hair[0].transform.position.y + m_height, transform.position.z), transform.rotation, transform);
            hair.GetComponent<HairController>().StartMoving();
            m_hair.Insert(0, hair);
            if (m_queueStopSpawn)
            {
                m_stopSpawn = true;
                m_stopMovingHairEvent.Invoke();
            }
        }
    }
}
