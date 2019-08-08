using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerClimbDeath : MonoBehaviour
{
    [SerializeField]
    private PlayerData m_playerData;
    [SerializeField]
    private Renderer m_tempBody;

    private void Start()
    {
        //if (m_playerData.Character.Mesh != null)
        //    m_tempBody.GetComponent<MeshFilter>().mesh = m_playerData.Character.Mesh;
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
        m_playerData.TowerClimbData.isDead = true;
        GetComponent<TowerClimbPlayerController>().OnDeath();
        Destroy(gameObject);
    }
}
