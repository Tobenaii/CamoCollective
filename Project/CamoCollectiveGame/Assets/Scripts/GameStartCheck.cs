using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartCheck : MonoBehaviour
{
    [SerializeField]
    private PlayerData m_playerData;

    private void Awake()
    {
        if (!m_playerData.IsPlaying)
            gameObject.SetActive(false);

    }
}
