using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoultryBashPowerup : MonoBehaviour
{
    [Header("Data")]
    [SerializeField]
    private GameObjectPool m_objectPool;
    [SerializeField]
    private GameObjectListSet m_gameObjectListSet;

    private void Start()
    {
        m_gameObjectListSet.Add(gameObject);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        m_gameObjectListSet.Remove(gameObject);
    }

    public virtual void TriggerEnter(PoultryBasher basher) { }

    public virtual void TriggerStay(PoultryBasher basher) { }

    public virtual void TriggerExit(PoultryBasher basher) { }

    public void Destroy()
    {
        m_objectPool.DestroyObject(gameObject);
    }
}
