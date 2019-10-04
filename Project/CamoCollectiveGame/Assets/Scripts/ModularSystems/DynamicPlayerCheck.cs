using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicPlayerCheck : MonoBehaviour
{
    [SerializeField]
    private PlayerData m_player;
    [SerializeField]
    private GameObject m_gameObject;
    [SerializeField]
    private bool m_checkForCharacterSelected;

    // Update is called once per frame
    void Update()
    {
        if (m_checkForCharacterSelected && m_player.Character != null)
        {
            m_gameObject.SetActive(false);
            return;
        }

        if (m_player.IsPlaying)
            m_gameObject.SetActive(true);
        else
            m_gameObject.SetActive(false);


    }
}
