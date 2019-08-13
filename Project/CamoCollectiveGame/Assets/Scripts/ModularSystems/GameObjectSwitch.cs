using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectSwitch : MonoBehaviour
{
    [SerializeField]
    private bool m_disableOnWake;
    [SerializeField]
    private bool m_enableOnAwake;
    [SerializeField]
    private GameObject m_gameObject;

    private void Awake()
    {
        if (m_disableOnWake)
            m_gameObject.SetActive(false);
        if (m_enableOnAwake)
            m_gameObject.SetActive(true);
    }

    public void EnableGameObject()
    {
        m_gameObject.SetActive(true);
    }

    public void DisableGameObject()
    {
        m_gameObject.SetActive(false);
    }

    public void SwitchGameObject()
    {
        m_gameObject.SetActive(!m_gameObject.activeSelf);
    }
}
