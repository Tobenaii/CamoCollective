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
    [SerializeField]
    private List<GameObject> m_gameObjects;

    private void Awake()
    {
        if (m_disableOnWake)
            DisableGameObject();
        if (m_enableOnAwake)
            EnableGameObject();
    }

    public void EnableGameObject()
    {
        foreach (GameObject obj in m_gameObjects)
            obj.SetActive(true);
        if (m_gameObject)
            m_gameObject.SetActive(true);
    }

    public void DisableGameObject()
    {
        foreach (GameObject obj in m_gameObjects)
            obj.SetActive(false);
        if (m_gameObject)
            m_gameObject.SetActive(false);
    }

    public void SwitchGameObject()
    {
        m_gameObject.SetActive(!m_gameObject.activeSelf);
        foreach (GameObject obj in m_gameObjects)
            obj.SetActive(!obj.activeSelf);
    }
}
