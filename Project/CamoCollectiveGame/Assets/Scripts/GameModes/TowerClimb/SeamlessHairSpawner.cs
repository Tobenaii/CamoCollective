using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeamlessHairSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject m_hairPrefab;
    [SerializeField]
    private int m_initAmmount;
    private float m_height;
    private void Awake()
    {
        m_height = m_hairPrefab.GetComponent<Renderer>().bounds.size.y;
        for (int i = 1; i <= m_initAmmount; i++)
        {
            HairController hair = Instantiate(m_hairPrefab, new Vector3(transform.position.x, transform.position.y + (m_height) * (i), transform.position.z), transform.rotation, transform).GetComponent<HairController>();
        }
    }
}
