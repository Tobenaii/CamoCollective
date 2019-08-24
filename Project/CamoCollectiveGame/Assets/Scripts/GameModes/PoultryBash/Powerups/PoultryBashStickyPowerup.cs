using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoultryBashStickyPowerup : PoultryBashPowerup
{
    [SerializeField]
    private Animator m_animator;
    [SerializeField]
    private float m_speedScale;
    [SerializeField]
    private float m_aliveTime;

    private float m_timer;
    private bool m_isShrinking;

    private void OnEnable()
    {
        m_timer = m_aliveTime;
        m_isShrinking = false;
    }

    private void Update()
    {
        m_aliveTime -= Time.deltaTime;
        if (m_aliveTime < 0 && !m_isShrinking)
        {
            m_isShrinking = true;
            m_animator.SetTrigger("Dissapear");
        }
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        m_animator.SetTrigger("Splat");
    }

    public override void TriggerEnter(PoultryBasher basher)
    {
        basher.ScaleSpeed(m_speedScale);
    }

    public override void TriggerExit(PoultryBasher basher)
    {
        basher.ScaleSpeed(1);
    }
}
