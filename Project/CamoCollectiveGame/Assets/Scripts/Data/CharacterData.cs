using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/CharacterData")]
public class CharacterData : ScriptableObject
{
    [SerializeField]
    private Sprite m_icon;

    public Sprite GetIcon()
    {
        return m_icon;
    }
}
