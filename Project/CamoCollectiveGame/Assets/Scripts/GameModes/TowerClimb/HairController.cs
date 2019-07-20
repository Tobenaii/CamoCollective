using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_hairPrefab;
    [SerializeField]
    private float m_moveSpeed;
    [SerializeField]
    private GameObject m_obstaclePrefab;
    private bool m_dontSpawnHair;
    private float m_initPosY;
    private Renderer m_renderer;
    private bool m_moveHair;
    private GameObject m_obstacle;

    private void Awake()
    {
        m_initPosY = transform.position.y;
        m_renderer = GetComponent<Renderer>();
        m_moveHair = false;
        m_dontSpawnHair = false;
    }

    public void StartMoving()
    {
        m_moveHair = true;
    }
    public void DontSpawnHair()
    {
        m_dontSpawnHair = true;
    }

    private void Update()
    {
        if (!m_moveHair)
            return;
        transform.position = new Vector3(transform.position.x, transform.position.y - m_moveSpeed * Time.deltaTime, transform.position.z);
        if (transform.position.y < -40)
        {
            Destroy(gameObject);
        }
        if (m_dontSpawnHair)
            return;
        if (transform.position.y < m_initPosY - m_renderer.bounds.size.y)
        {
            HairController hair = Instantiate(m_hairPrefab, new Vector3(transform.position.x, transform.position.y + m_renderer.bounds.size.y, transform.position.z), transform.rotation, transform.parent).GetComponent<HairController>();
            hair.StartMoving();
            m_dontSpawnHair = true;
            if (m_moveHair)
            {
                float spawn = Random.Range(0, 100);
                if (spawn > 20)
                    return;
                float xPos = Random.Range(hair.transform.position.x - m_renderer.bounds.extents.x, hair.transform.position.x + m_renderer.bounds.extents.x);
                float yPos = Random.Range(hair.transform.position.y - m_renderer.bounds.extents.y, hair.transform.position.y + m_renderer.bounds.extents.y);
                m_obstacle = Instantiate(m_obstaclePrefab, new Vector3(xPos, yPos, hair.transform.position.z), Quaternion.identity, transform);
            }
        }
    }
}
