using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectListAdd : MonoBehaviour
{
    [SerializeField]
    private GameObjectListSet m_listSet;
    [SerializeField]
    private bool m_addOnAwake;
    [SerializeField]
    private bool m_removeOnDestroy = true;

    private void Awake()
    {
        if (m_addOnAwake)
            m_listSet.Add(gameObject);
    }

    public void AddToList()
    {
        m_listSet.Add(gameObject);
    }

    private void OnDestroy()
    {
        if (m_removeOnDestroy)
            m_listSet.Remove(gameObject);
    }
}
