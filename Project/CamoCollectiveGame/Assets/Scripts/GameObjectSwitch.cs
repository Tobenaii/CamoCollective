using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectSwitch : MonoBehaviour
{
    [SerializeField]
    private GameObject m_gameObject;

    public void EnableGameObject()
    {
        m_gameObject.SetActive(true);
    }

    public void DisableGameObject()
    {
        m_gameObject.SetActive(false);
    }
}
