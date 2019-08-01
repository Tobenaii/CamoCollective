using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairController : MonoBehaviour
{
    [SerializeField]
    private float m_moveTime;
    private Renderer m_renderer;
    private bool m_moveHair;
    private float m_offset;
    private TimeLerper m_lerper;

    private void Awake()
    {
        m_renderer = GetComponent<Renderer>();
        m_moveHair = false;
        m_lerper = new TimeLerper();
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
        if (m_offset == 0)
        {
            m_lerper.Reset();
            m_offset = 1;
        }
        m_renderer.material.SetTextureOffset("_MainTex", new Vector2(0, m_offset));
        m_offset = m_lerper.Lerp(1, 0, m_moveTime);
    }
}
