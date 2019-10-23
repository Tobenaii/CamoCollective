using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBarrel : MonoBehaviour
{
    [SerializeField]
    private float m_explosionForce;
    [SerializeField]
    private float m_explosionRadius;
    [SerializeField]
    private float m_playerKnockback;
    [SerializeField]
    private GameObjectListSet m_playerGameObjects;
    [SerializeField]
    private GameObjectPool m_barrelPool;

    private Rigidbody m_rb;
    private Rigidbody[] m_children;

    private List<Vector3> m_positions = new List<Vector3>();
    private List<Quaternion> m_rotations = new List<Quaternion>();

    private void Start()
    {
        m_children = GetComponentsInChildren<Rigidbody>();
        m_rb = GetComponent<Rigidbody>();
        for (int i = 1; i < m_children.Length; i++)
        {
            m_positions.Add(m_children[i].transform.localPosition);
            m_rotations.Add(m_children[i].transform.localRotation);
        }
    }

    private void OnEnable()
    {
        for (int i = 1; i < m_children.Length; i++)
        {
            m_children[i].transform.localPosition = m_positions[i - 1];
            m_children[i].transform.localRotation = m_rotations[i - 1];
        }
        foreach (Rigidbody rb in m_children)
            rb.isKinematic = true;
        m_rb.isKinematic = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ring"))
            Explode();
    }

    private void Explode()
    {
        foreach (Rigidbody rb in m_children)
        {
            rb.isKinematic = false;
            rb.AddForce((rb.transform.position - transform.position).normalized * m_explosionForce, ForceMode.Impulse);
        }
        m_rb.isKinematic = true;

        foreach (GameObject player in m_playerGameObjects.List)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < m_explosionRadius)
            {
                Rigidbody rb = player.GetComponent<Rigidbody>();
                rb.velocity = (rb.transform.position - transform.position).normalized * m_playerKnockback;
            }
        }
    }

    public void OnNewRound()
    {
        m_barrelPool.DestroyObject(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, m_explosionRadius);
    }
}
