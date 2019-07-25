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
        foreach (Image image in m_images)
        {
            if (m_towerClimbers[index].player != null && m_towerClimbers[index].player.IsPlaying())
            {
                if (m_towerClimbers[index].isDead)
                    image.gameObject.SetActive(false);
                else
                {
                    image.gameObject.SetActive(true);
                    image.sprite = m_towerClimbers[index].player.GetCharacter().GetIcon();
                    image.SetNativeSize();
                }
                index++;
            }
            else
                image.gameObject.SetActive(false);
        }
    }
}
