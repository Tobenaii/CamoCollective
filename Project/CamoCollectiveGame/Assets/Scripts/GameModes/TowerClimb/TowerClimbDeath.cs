using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerClimbDeath : MonoBehaviour
{
    private TowerClimber m_climber;

    public void SetClimber(TowerClimber climber)
    {
        m_climber = climber;
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
    }
}
