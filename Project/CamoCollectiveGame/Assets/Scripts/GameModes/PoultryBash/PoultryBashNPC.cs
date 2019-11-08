using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoultryBashNPC : MonoBehaviour
{
    [SerializeField]
    private FloatValue m_dabWave;

    private bool m_hasDabbed;

    private Animator m_animator;
    private Material m_material;

    private float m_assScratchTimer; 

    private void Awake()
    {
        m_animator = GetComponentInChildren<Animator>();
        m_animator.speed = Random.Range(1.0f, 1.5f);
        GetRandomAssScratch();
    }

    private void GetRandomAssScratch()
    {
        m_assScratchTimer = Random.Range(5, 30);
    }

    public void Cheer()
    {
        m_animator.SetTrigger("Cheer" + Random.Range(1, 5).ToString());
    }

    // Start is called before the first frame update
    void Start()
    {
        //transform.position += transform.right * Random.Range(-0.5f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        //if (!m_hasDabbed && ("Column" + Mathf.Floor(m_dabWave.Value).ToString()) == transform.parent.parent.name)
        //{
        //    m_hasDabbed = true;
        //    m_animator.SetTrigger("Dab");
        //}
        m_assScratchTimer -= Time.deltaTime;
        if (m_assScratchTimer <= 0)
        {
            int merp = Random.Range(0, 2);
            if (merp == 0)
                m_animator.SetTrigger("AssScratch");
            else
                m_animator.SetTrigger("Dab");
            GetRandomAssScratch();
        }
    }
}
