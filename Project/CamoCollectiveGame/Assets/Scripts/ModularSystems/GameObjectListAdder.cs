using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectListAdder : MonoBehaviour
{
    [SerializeField]
    private GameObjectListSet m_listSet;
    [SerializeField]
    private bool m_atIndex;
    [SerializeField]
    private int m_index;

    private void Awake()
    {
        if (m_atIndex)
        {
            while (m_listSet.Count < m_index + 1)
            {
                m_listSet.Add(null);
            }
            m_listSet.Insert(m_index, gameObject);
            return;
        }
        m_listSet.Add(gameObject);

    }
}
