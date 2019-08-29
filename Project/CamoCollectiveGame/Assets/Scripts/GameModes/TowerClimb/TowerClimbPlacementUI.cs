using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TowerClimbPlacementUI : MonoBehaviour
{
    private class PlacementSlot
    {
        public FloatValue yValues;
        public float yPos { get { return yValues.GetValue(index); } private set { } }
        public int index;
    }

    [SerializeField]
    private List<PlayerData> m_playerData;
    [SerializeField]
    private BoolValue m_isDeadValues;
    [SerializeField]
    private FloatValue m_yPosValues;
    [SerializeField]
    private List<Image> m_images;

    private List<PlacementSlot> m_placementSlots = new List<PlacementSlot>();

    private void Start()
    {
        for (int i = 0; i < m_playerData.Count; i++)
        {
            if (m_playerData[i].IsPlaying)
            {
                m_placementSlots.Add(new PlacementSlot() { yValues = m_yPosValues, index = i });
            }
        }
    }

    private void LateUpdate()
    {
        m_placementSlots = m_placementSlots.OrderBy(x => -x.yPos).ToList();

        for (int i = 0; i < m_placementSlots.Count; i++)
        {
            m_images[i].sprite = m_playerData[m_placementSlots[i].index].Character.Icon;
            m_images[i].SetNativeSize();
            //if (m_isDeadValues.GetValue(m_placementSlots[i].index))
            //    m_images[i].color = Color.red;
        }
    }
}
