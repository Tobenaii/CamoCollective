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
    public Sprite Icon { get { return m_icon; } private set { } }
    public Color TempColour { get { return m_tempColour; } private set { } }
}
