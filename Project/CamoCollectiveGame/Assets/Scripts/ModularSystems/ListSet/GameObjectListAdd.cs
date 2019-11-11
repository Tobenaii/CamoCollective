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
        if (CheckError())
            return;
        if (m_addOnAwake)
            m_listSet.Add(gameObject);
    }

    private bool CheckError()
    {
        if (!m_listSet)
        {
            Debug.LogError("Null list set on " + gameObject.name);
            return true;
        }
        return false;
    }

    public void AddToList()
    {
        if (CheckError())
            return;
        m_listSet.Add(gameObject);
    }

    private void OnDestroy()
    {
        if (CheckError())
            return;
        if (m_removeOnDestroy)
            m_listSet.Remove(gameObject);
    }
}
