using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField]
    private CharacterData m_character;
    [SerializeField]
    private Vector3Value m_playerCursors;

    private RectTransform m_rect;
    private Image[] m_images;

    private PlayerData m_assignedPlayer;
    private Button m_button;

    private void Start()
    {
        m_rect = GetComponent<RectTransform>();
        m_images = GetComponentsInChildren<Image>();
        m_button = GetComponentInChildren<Button>();
    }

    private void Update()
    {
        if (m_assignedPlayer == null)
            return;
        if (m_assignedPlayer.Character == null)
        {
            foreach (Image image in m_images)
                image.color = new Color(1,1,1,1);
            m_button.interactable = true;
            m_assignedPlayer = null;
        }
    }

    public void OnHoverEnter(PlayerData player)
    {
        player.SetGhostCharacter(m_character);
        Debug.Log("Setting " + player.name + " Ghost Character to " + m_character.name);
    }

    public void OnHoverExit(PlayerData player)
    {
        player.SetGhostCharacter(null);
        Debug.Log("Setting " + player.name + " Ghost Character to null");
    }

    public void OnCursorClick(PlayerData player)
    {
        player.Character = m_character;
        m_assignedPlayer = player;
        Debug.Log("Setting " + player.name + "Character to " + m_character.name);
        Color c = player.IndicatorColour;
        foreach (Image image in m_images)
            image.color = new Color(c.r, c.g, c.b, 0.75f);
    }
}
