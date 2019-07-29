using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerClimbPlacementUI : MonoBehaviour
{
    [SerializeField]
    private List<Image> m_images;
    [SerializeField]
    private List<TowerClimber> m_towerClimbers;

    private void Update()
    {
        m_towerClimbers.Sort();

        int index = 0;
        for (int i = 0; i < m_towerClimbers.Count; i++)
        {
            if (m_towerClimbers[i].player != null && m_towerClimbers[i].player.IsPlaying())
            {
                if (m_towerClimbers[i].isDead)
                    m_images[index].gameObject.SetActive(false);
                else
                {
                    m_images[index].gameObject.SetActive(true);
                    m_images[index].sprite = m_towerClimbers[i].player.GetCharacter().Icon;
                    m_images[index].SetNativeSize();
                }
            }
            else
                m_images[index].gameObject.SetActive(false);
            index++;
        }
    }
}
