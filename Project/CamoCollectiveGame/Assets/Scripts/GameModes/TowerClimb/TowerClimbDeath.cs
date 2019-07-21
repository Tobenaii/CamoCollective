using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerClimbDeath : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Vector3 vport = Camera.main.WorldToViewportPoint(transform.position);
        //if (vport.y < 0)
        //    Debug.Log("DEAD!");
    }
}
