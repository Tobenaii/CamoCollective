using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoultryBashPowerup : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField]
    private GameObjectEvent m_dynamicCameraAddEvent;
    [SerializeField]
    private GameObjectEvent m_dynamicCameraRemoveEvent;

    [Header("Data")]
    [SerializeField]
    private GameObjectPool m_objectPool;

    private void Awake()
    {
        m_dynamicCameraAddEvent.Invoke(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        m_dynamicCameraRemoveEvent.Invoke(gameObject);
    }

    public abstract void ApplyPowerup(PoultryBasher basher);

    public void Destroy()
    {
        m_objectPool.DestroyObject(gameObject);
    }
}
