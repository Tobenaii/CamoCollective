using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField]
    private GameObjectEvent m_changeCameraTargetEvent;

    private void Start()
    {
        m_changeCameraTargetEvent.Invoke(gameObject);
    }
}
