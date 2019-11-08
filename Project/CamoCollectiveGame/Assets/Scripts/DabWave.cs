using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DabWave : MonoBehaviour
{
    [SerializeField]
    private FloatValue m_dabWave;

    private bool m_startDab;

    private void Start()
    {
        m_dabWave.Value = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_startDab = true;
        }
    }

    private void LateUpdate()
    {
        if (m_startDab)
            m_dabWave.Value += 8 * Time.deltaTime;
    }
}
