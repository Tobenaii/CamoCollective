using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastMinuteFix : MonoBehaviour
{
    [SerializeField]
    private BoolValue m_inStoryMode;
    [SerializeField]
    private PlayerData m_playerData;

    private void Update()
    {
        if (m_inStoryMode.Value && !m_playerData.IsPlaying)
            gameObject.SetActive(false);
    }
}
