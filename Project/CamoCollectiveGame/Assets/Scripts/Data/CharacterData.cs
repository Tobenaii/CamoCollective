﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/CharacterData")]
public class CharacterData : ScriptableObject
{
    [SerializeField]
    private Sprite m_icon;
    [SerializeField]
    private Color m_tempColour;
    [SerializeField]
    private GameObject m_chonkJoustingCharacter;

    [System.NonSerialized]
    public bool inUse;

    public Sprite Icon { get { return m_icon; } private set { } }
    public Color TempColour { get { return m_tempColour; } private set { } }
    public GameObject ChonkJoustingCharacter { get { return m_chonkJoustingCharacter; } private set { } }
}
