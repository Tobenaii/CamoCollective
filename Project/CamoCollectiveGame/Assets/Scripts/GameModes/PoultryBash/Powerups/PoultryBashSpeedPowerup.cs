using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoultryBashSpeedPowerup : PoultryBashPowerup
{
    [SerializeField]
    private float m_speedScale;
    [SerializeField]
    private float m_seconds;


    public override void TriggerEnter(PoultryBasher basher)
    {
        basher.ScaleSpeedForSeconds(m_speedScale, m_seconds);
        Destroy();
    }
}
