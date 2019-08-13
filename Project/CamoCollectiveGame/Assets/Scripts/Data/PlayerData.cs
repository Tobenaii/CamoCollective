using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/PlayerData")]
public class PlayerData : ScriptableObject
{
    [System.NonSerialized]
    private CharacterData m_currentCharacter;
    [System.NonSerialized]
    private bool m_isPlaying;

    public CharacterData Character { get { return m_currentCharacter; } set { m_currentCharacter = value; } }
    public bool IsPlaying { get { return m_isPlaying; } set { m_isPlaying = value; } }

    public void RemoveCharacter()
    {
        if (m_currentCharacter == null)
            return;
        m_currentCharacter.inUse = false;
        m_currentCharacter = null;
    }
}
