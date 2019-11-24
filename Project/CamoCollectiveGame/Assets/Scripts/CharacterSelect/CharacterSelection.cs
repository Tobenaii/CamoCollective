using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField]
    private List<PlayerData> m_players;
    [SerializeField]
    private CharacterData m_character;
    [SerializeField]
    private Vector3Value m_playerCursors;

    private RectTransform m_rect;
    [SerializeField]
    private Image m_image;

    private PlayerData m_assignedPlayer;
    private Button m_button;

    private void Start()
    {
        m_rect = GetComponent<RectTransform>();
        m_button = GetComponentInChildren<Button>();

        foreach (PlayerData player in m_players)
        {
            if (player.IsPlaying && player.Character == m_character)
            {
                m_button.interactable = false;
                OnCursorClick(player);
                break;
            }
        }
    }

    private void Update()
    {
        if (m_assignedPlayer == null)
            return;
        if (m_assignedPlayer.Character == null)
        {
            m_image.color = new Color(1,1,1,1);
            m_button.interactable = true;
            m_assignedPlayer = null;
        }
    }

    public void OnHoverEnter(PlayerData player)
    {
        player.SetGhostCharacter(m_character);
    }

    public void OnHoverExit(PlayerData player)
    {
        player.SetGhostCharacter(null);
    }

    public void OnCursorClick(PlayerData player)
    {
        player.Character = m_character;
        m_assignedPlayer = player;
        Color c = Color.black;
        m_image.color = new Color(c.r, c.g, c.b, 0.75f);
    }
}
