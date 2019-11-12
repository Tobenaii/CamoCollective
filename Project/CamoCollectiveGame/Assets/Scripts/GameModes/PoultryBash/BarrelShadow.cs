using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelShadow : MonoBehaviour
{
    [SerializeField]
    private float m_growTime;
    [SerializeField]
    private Vector3 m_finalScale;
    [SerializeField]
    private GameObjectPool m_shadowPool;

    private TimeLerper m_lerper = new TimeLerper();
    private Vector3 m_initScale;

    private void Start()
    {
        m_initScale = Vector3.zero;
        transform.localScale = m_initScale;
    }

    private void OnEnable()
    {
        m_lerper.Reset();
    }

    private void Update()
    {
        transform.localScale = m_lerper.Lerp(m_initScale, m_finalScale, m_growTime);
    }
}
