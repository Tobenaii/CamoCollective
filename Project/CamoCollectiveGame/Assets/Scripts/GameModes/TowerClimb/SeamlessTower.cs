using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeamlessTower : MonoBehaviour
{
    [SerializeField]
    private FloatValue m_fallSpeed;
    [SerializeField]
    private Material m_towerMat;
    private bool m_started;

    public void StartMoving()
    {
        m_started = true;
    }

    public void StopMoving()
    {
        m_started = false;
    }

    private void LateUpdate()
    {
        if (!m_started)
            return;
        m_towerMat.mainTextureOffset += Vector2.up * m_fallSpeed.Value * (Time.deltaTime / 11);
    }



}
