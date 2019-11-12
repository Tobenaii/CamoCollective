using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBanner : MonoBehaviour
{
    [SerializeField]
    private PlayerData m_player;

    [SerializeField]
    private Image m_image;
   
    
    public void Start()
    {
        m_image = GetComponent<Image>();
        m_image.color = m_player.BannerColour;
    }
}
