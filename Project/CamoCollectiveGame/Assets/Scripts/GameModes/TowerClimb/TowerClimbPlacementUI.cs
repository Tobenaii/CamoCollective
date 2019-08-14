using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TowerClimbPlacementUI : MonoBehaviour
{
    private class PlacementSlot
    {
        public float yPos;
        public Sprite sprite;
    }

    [SerializeField]
    private List<PlayerData> m_playerData;
    [SerializeField]
    private FloatValue m_yPosValues;
    [SerializeField]
    private List<Image> m_images;
    private List<PlacementSlot> m_placementSlots = new List<PlacementSlot>();

    private void Sort(PlayerData player, int yIndex, int index = 0)
    {
        if (m_yPosValues.GetValue(yIndex) >= m_placementSlots[index].yPos || index == m_placementSlots.Count - 1)
        {
            if (index + 1 < m_placementSlots.Count)
            {
                m_placementSlots[index + 1].sprite = m_placementSlots[index].sprite;
                m_placementSlots[index + 1].yPos = m_placementSlots[index].yPos;
            }

            m_placementSlots[index].sprite = player.Character.Icon;
            m_placementSlots[index].yPos = m_yPosValues.GetValue(yIndex);
        }
        else
            Sort(player, yIndex, index + 1);
    }

    private void Start()
    {
        for (int i = 0; i < m_playerData.Count; i++)
        {
            if (m_playerData[i].IsPlaying)
            {
                m_placementSlots.Add(new PlacementSlot() { yPos = m_yPosValues.GetValue(i), sprite = m_playerData[i].Character.Icon });
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < m_placementSlots.Count; i++)
            m_placementSlots[i].yPos = m_yPosValues.GetValue(i);

        for (int i = 0; i < m_placementSlots.Count; i++)
            Sort(m_playerData[i], i);

        for (int i = 0; i < m_placementSlots.Count; i++)
        {
            m_images[i].sprite = m_placementSlots[i].sprite;
            m_images[i].SetNativeSize();
        }
    }
}
