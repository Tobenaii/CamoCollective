using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoultryBashStrengthPowerup : PoultryBashPowerup
{
    [SerializeField]
    private float m_strengthScale;
    [SerializeField]
    private float m_seconds;

    public override void ApplyPowerup(PoultryBasher basher)
    {
        basher.ScaleKnockbackForSeconds(m_strengthScale, m_seconds);
    }
}
