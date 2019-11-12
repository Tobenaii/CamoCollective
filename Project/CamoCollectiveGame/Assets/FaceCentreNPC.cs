using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCentreNPC : MonoBehaviour
{
    [SerializeField]
    private Transform m_lookTarget;


    private void Awake()
    {
        transform.LookAt(new Vector3(m_lookTarget.transform.position.x, transform.position.y, m_lookTarget.transform.position.z));
    }
}
