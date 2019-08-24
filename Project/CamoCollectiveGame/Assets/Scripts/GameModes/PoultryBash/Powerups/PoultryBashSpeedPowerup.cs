using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoultryBashSpeedPowerup : PoultryBashPowerup
{
    [SerializeField]
    private float m_speedScale;
    [SerializeField]
    private float m_seconds;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }

    public override void ApplyPowerup(PoultryBasher basher)
    {
        basher.ScaleSpeedForSeconds(m_speedScale, m_seconds);
    }
}
