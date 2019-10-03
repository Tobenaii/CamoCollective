using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeaver : MonoBehaviour
{
    [SerializeField]
    private PlayerData m_playerData;
    [SerializeField]
    private FloatReference m_currentPlayerIndex;

    public void Leave()
    {
        if (m_playerData.IsPlaying && m_playerData.Character != null)
            m_playerData.Character = null;
        else
        {
            m_playerData.IsPlaying = false;
            m_currentPlayerIndex.Value = -1;
        }
    }
}
