using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChonkSpawner : MonoBehaviour
{
    [SerializeField]
    private List<Transform> m_spawns;
    [SerializeField]
    private List<PlayerData> m_playerData;
    [SerializeField]
    private List<ChonkJouster> m_jousters;
    [SerializeField]
    private GameObject m_chonkPrefab;
    [SerializeField]
    private Transform m_respawnTransform;


    private void Awake()
    {
        int index = 0;
        foreach (PlayerData player in m_playerData)
        {
            if (player.IsPlaying())
            {
                ChonkJoustingData data = Instantiate(m_chonkPrefab).GetComponent<ChonkJoustingData>();
                data.GetComponent<ChonkJoustingDeath>().SetRespawnTransform(m_respawnTransform);
                data.gameObject.transform.SetParent(transform);
                data.transform.position = m_spawns[index].position;
                data.transform.rotation = m_spawns[index].rotation;
                m_jousters[index].player = player;
                data.SetChonkJouster(m_jousters[index]);
                index++;
            }
        }
        for (int i = index; i < m_jousters.Count; i++)
        {
            m_jousters[index].player = null;
        }
    }
}
