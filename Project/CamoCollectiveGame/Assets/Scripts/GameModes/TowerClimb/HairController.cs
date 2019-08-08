using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairController : MonoBehaviour
{
    [SerializeField]
    private float m_moveTime;
    [SerializeField]
    private FloatValue m_scale;
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
        m_renderer.material.SetTextureOffset("_MainTex", new Vector2(0, m_offset));
        m_offset -= Time.deltaTime * m_scale.Value;
    }
}
