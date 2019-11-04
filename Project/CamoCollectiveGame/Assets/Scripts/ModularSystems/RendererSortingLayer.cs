using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RendererSortingLayer : MonoBehaviour
{
    [SerializeField]
    private Renderer m_renderer;
    [SerializeField]
    private string m_layer;
    [SerializeField]
    private int m_sortingOrder;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        m_renderer.sortingLayerName = m_layer;
        m_renderer.sortingOrder = m_sortingOrder;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
