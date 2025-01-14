﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/PlayerData")]
public class PlayerData : ScriptableObject
{
    [SerializeField]
    private int m_playerNumber;
    [SerializeField]
    private CharacterData m_forcedCharacter;
    [System.NonSerialized]
    private CharacterData m_currentCharacter;
    [System.NonSerialized]
    private CharacterData m_ghostCharacter;
    [System.NonSerialized]
    private bool m_isPlaying;
    [SerializeField]
    private Color m_indicatorColour;
    [SerializeField]
    private Color32 m_bannerColour;
    [System.NonSerialized]
    private bool m_justJoined;
    [System.NonSerialized]
    private bool m_tempIsPlaying;

    public int PlayerNumber => m_playerNumber;
    public CharacterData Character { get { return m_currentCharacter; } set { if (m_currentCharacter != null) m_currentCharacter.inUse = false; m_currentCharacter = value; if (m_currentCharacter != null) m_currentCharacter.inUse = true; } }
    public CharacterData ForcedCharacter { get { return m_forcedCharacter; } private set { } }
    public bool IsPlaying { get { return m_isPlaying; } set { m_isPlaying = value; } }
    public Color IndicatorColour { get { return m_indicatorColour; } private set { } }
    public Color BannerColour { get { return m_bannerColour; } private set { } }
    public CharacterData GhostCharacter { get { return m_ghostCharacter; } private set { } }
    public bool JustJoined { get { return m_justJoined; } set { m_justJoined = value; } }
    public bool TempIsPlaying { get { return m_tempIsPlaying; } set { m_tempIsPlaying = value; } }

    public void SetGhostCharacter(CharacterData character)
    {
        m_ghostCharacter = character;
    }

    public void RemoveCharacter()
    {
        if (m_currentCharacter == null)
            return;
        m_currentCharacter.inUse = false;
        m_currentCharacter = null;
    }
}
