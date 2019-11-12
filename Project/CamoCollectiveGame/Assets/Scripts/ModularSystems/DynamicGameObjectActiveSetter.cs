using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicGameObjectActiveSetter : MonoBehaviour
{
    [SerializeField]
    private GameObject m_gameObject;
    [SerializeField]
    private BoolReference m_boolReference;
    private bool m_overridden;

    // Update is called once per frame
    void Update()
    {
        if (m_overridden)
            return;
        if (m_gameObject.activeSelf && !m_boolReference.Value)
            m_gameObject.SetActive(false);
        else if (!m_gameObject.activeSelf && m_boolReference.Value)
            m_gameObject.SetActive(true);
    }

    public void Override(bool value)
    {
        m_overridden = true;
        m_gameObject.SetActive(value);
    }
}
