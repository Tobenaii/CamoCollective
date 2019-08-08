using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TowerClimbPlacementUI : MonoBehaviour
{
    [SerializeField]
    private List<PlayerData> m_playerData;
    [SerializeField]
    private List<Image> m_images;

    private void Update()
    {
        m_playerData = m_playerData.OrderBy(x => -x.TowerClimbData.yPos).ToList<PlayerData>();

        int image = 0;
        for (int i = 0; i < m_playerData.Count; i++)
        {
            if (!m_playerData[i].IsPlaying)
                m_images[i].gameObject.SetActive(false);
            else
            {
                Debug.Log(m_playerData[i].TowerClimbData.yPos);
                m_images[image].sprite = m_playerData[i].Character.Icon;
                m_images[image].SetNativeSize();
                image++;
             }
        }
    }
}
