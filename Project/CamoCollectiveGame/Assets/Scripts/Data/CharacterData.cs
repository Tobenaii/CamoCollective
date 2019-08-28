using System.Collections;
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
    [SerializeField]
    private GameObject m_towerClimbCharacter;
    [SerializeField]
    private GameObject m_poultryBashCharacter;

    [System.NonSerialized]
    public bool inUse;

    public Sprite Icon { get { return m_icon; } private set { } }
    public Color TempColour { get { return m_tempColour; } private set { } }
    public GameObject ChonkJoustingCharacter { get { return m_chonkJoustingCharacter; } private set { } }
    public GameObject TowerClimbCharacter { get { return m_towerClimbCharacter; } private set { } }
    public GameObject PoultryBashCharacter { get { return m_poultryBashCharacter; } private set { } }
}
