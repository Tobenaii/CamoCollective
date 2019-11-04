using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRing : MonoBehaviour
{
    [SerializeField]
    private PlayerData m_player;

    [SerializeField]
    private SpriteRenderer m_spriteRenderer;


    public void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_spriteRenderer.color = m_player.IndicatorColour;
    }    
}
