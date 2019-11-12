using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnChecker : MonoBehaviour
{
    [SerializeField]
    private int m_playerAmmount;
    [SerializeField]
    private List<PlayerData> m_playerData;
    [SerializeField]
    private List<GameObject> m_playerObjects;
    [SerializeField]
    private List<Transform> m_spawnTransforms;
    [SerializeField]
    private BoolValue m_spawnTempPlayers;
    [SerializeField]
    private GameObject m_gameObjectToEnable;

    private void OnValidate()
    {
        while (m_spawnTransforms.Count < m_playerAmmount)
            m_spawnTransforms.Add(null);
        while (m_spawnTransforms.Count > m_playerAmmount)
            m_spawnTransforms.RemoveAt(m_spawnTransforms.Count - 1);
    }

    private void Awake()
    {
        int players = 0;
        foreach (PlayerData player in m_playerData)
            players += Convert.ToInt32(((m_spawnTempPlayers.Value)?player.TempIsPlaying:player.IsPlaying));
        if (players == m_playerAmmount)
        {
            if (m_gameObjectToEnable)
                m_gameObjectToEnable.SetActive(true);
            int index = 0;
            int spawnIndex = 0;
            foreach (PlayerData player in m_playerData)
            {
                if ((m_spawnTempPlayers.Value) ? player.TempIsPlaying : player.IsPlaying)
                {
                    GameObject p = Instantiate(m_playerObjects[index], m_spawnTransforms[spawnIndex]);
                    p.transform.localPosition = Vector3.zero;
                    p.transform.localRotation = Quaternion.identity;
                    spawnIndex++;
                }
                index++;
            }
        }
        else
        {
            if (m_gameObjectToEnable)
                m_gameObjectToEnable.SetActive(false);
        }
    }
}
