using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMaterial : MonoBehaviour
{
    [SerializeField]
    private Renderer m_renderer;
    [SerializeField]
    private List<Material> m_materials;

    private void Awake()
    {
        m_renderer.material = m_materials[Random.Range(0, m_materials.Count)];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
