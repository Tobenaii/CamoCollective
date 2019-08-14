using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairController : MonoBehaviour
{
    [SerializeField]
    private FloatValue m_moveSpeed;
    private Renderer m_renderer;
    private bool m_moveHair;
    private float m_offset;

    private void Awake()
    {
        m_renderer = GetComponent<Renderer>();
        m_moveHair = false;
    }

    public void StartMoving()
    {
        m_moveHair = true;
    }

    public void StopMoving()
    {
        m_moveHair = false;
    }

    private void Update()
    {
        if (!m_moveHair)
            return;
        transform.position += Vector3.down * m_moveSpeed.Value * Time.deltaTime;
    }
}
