using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerClimbDeath : MonoBehaviour
{
    [SerializeField]
    private PlayerData m_playerData;
    [SerializeField]
    private Renderer m_tempBody;

    [Header("Data")]
    [SerializeField]
    private BoolReference m_isDeadValue;

    private void Start()
    {
        m_tempBody.material.color = m_playerData.Character.TempColour;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vport = Camera.main.WorldToViewportPoint(transform.position);
        if (vport.y < 0)
            OnDeath();
    }

    private void OnDeath()
    {
        m_isDeadValue.Value = true;
        Destroy(gameObject);
    }
}
