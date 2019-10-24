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
    [SerializeField]
    private GameObjectPool m_shadowPool;
    [SerializeField]
    private float m_shadowYPos;
    [SerializeField]
    private float m_fallDelay;
    [SerializeField]
    private float m_shadowGrowSpeed;

    private Rigidbody m_rb;
    private Rigidbody[] m_children;

    private GameObject m_shadow;
    private bool m_exploded;

    private void Start()
    {

    }

    private void Update()
    {
        if (m_exploded)
            return;
        if (!m_shadow)
            return;
        m_shadow.transform.localScale = new Vector3(m_shadow.transform.localScale.x + 
            m_shadowGrowSpeed * Time.deltaTime, 1, m_shadow.transform.localScale.z + 
            m_shadowGrowSpeed * Time.deltaTime);
    }

    private void OnEnable()
    {
        m_children = GetComponentsInChildren<Rigidbody>();
        m_rb = GetComponent<Rigidbody>();
        m_children = GetComponentsInChildren<Rigidbody>();
        m_rb = GetComponent<Rigidbody>();
        foreach (Rigidbody rb in m_children)
            rb.isKinematic = true;
        m_rb.isKinematic = true;
        StartCoroutine(GetShadow());
        StartCoroutine(FallDelay());
    }  

    private IEnumerator GetShadow()
    {
        yield return new WaitForEndOfFrame();

        m_shadow = m_shadowPool.GetObject();
        m_shadow.transform.SetParent(transform.parent);
        m_shadow.transform.position = new Vector3(transform.position.x, m_shadowYPos, transform.position.z);
        m_shadow.transform.localScale = Vector3.zero;
        //m_playerGameObjects.Add(m_shadow);
    }

    private IEnumerator FallDelay()
    {
        float timer = m_fallDelay;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        m_rb.isKinematic = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ring"))
            Explode();
    }

    private void Explode()
    {
        m_exploded = true;
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
                if (rb)
                    rb.velocity = ((rb.transform.position - transform.position).normalized * m_playerKnockback) + (Vector3.up * 20);
            }
        }
        StartCoroutine(RemoveShadow());
    }

    public void RemoveShadowCheck()
    {
        if (!m_exploded)
            OnNewRound();
    }

    private IEnumerator RemoveShadow()
    {
        yield return new WaitForEndOfFrame();
        //m_playerGameObjects.Remove(m_shadow);
        m_shadowPool.DestroyObject(m_shadow);
    }

    public void OnNewRound()
    {
        StartCoroutine(DestroyAfterFrame());
        StartCoroutine(RemoveShadow());
    }

    private IEnumerator DestroyAfterFrame()
    {
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, m_explosionRadius);
    }
}
