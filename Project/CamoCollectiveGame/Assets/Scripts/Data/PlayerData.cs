using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/PlayerData")]
public class PlayerData : ScriptableObject
{
    [SerializeField]
    private int m_playerNum;
    [System.NonSerialized]
    private CharacterData m_currentCharacter;
    [System.NonSerialized]
    private int m_rulerScore;
    [System.NonSerialized]
    private bool m_isPlaying;

    public CharacterData GetCharacter()
    {
        return m_currentCharacter;
    }

    public void AddToScore(int value)
    {
        m_rulerScore += value;
    }

    public int GetRulerScore()
    {
        return m_rulerScore;
    }

    public void SetCharacter(CharacterData character)
    {
        if (m_currentCharacter != null)
            m_currentCharacter.inUse = false;
        m_currentCharacter = character;
        m_currentCharacter.inUse = true;
    }

    public void RemoveCharacter()
    {
        if (m_currentCharacter == null)
            return;
        m_currentCharacter.inUse = false;
        m_currentCharacter = null;
    }

    public void SetPlaying(bool playing)
    {
        m_isPlaying = playing;
    }

    public bool IsPlaying()
    {
        return m_isPlaying;
    }

    public int GetPlayerNum()
    {
        return m_playerNum;
    }
}
