using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerClimbDeath : MonoBehaviour
{
    private TowerClimber m_climber;
    [SerializeField]
    private Renderer m_tempBody;

    public void SetClimber(TowerClimber climber)
    {
        m_climber = climber;
        m_tempBody.material.color = climber.player.GetCharacter().TempColour;
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
        m_climber.isDead = true;
        GetComponent<TowerClimbPlayerController>().OnDeath();
        Destroy(gameObject);
    }
}
