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

    private void Start()
    {
        m_dynamicCameraAddEvent.Invoke(gameObject);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        m_dynamicCameraRemoveEvent.Invoke(gameObject);
    }

    public virtual void TriggerEnter(PoultryBasher basher) { }

    public virtual void TriggerStay(PoultryBasher basher) { }

    public virtual void TriggerExit(PoultryBasher basher) { }

    public void Destroy()
    {
        m_objectPool.DestroyObject(gameObject);
    }
}
