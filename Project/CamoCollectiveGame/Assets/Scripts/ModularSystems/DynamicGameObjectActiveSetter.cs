using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicGameObjectActiveSetter : MonoBehaviour
{
    [SerializeField]
    private bool m_inverse;
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
        bool trig = (m_inverse) ? !m_boolReference.Value : m_boolReference.Value;
        if (m_gameObject.activeSelf && !trig)
            m_gameObject.SetActive(false);
        else if (!m_gameObject.activeSelf && trig)
            m_gameObject.SetActive(true);
    }

    public void Override(bool value)
    {
        m_overridden = true;
        m_gameObject.SetActive(value);
    }
}
