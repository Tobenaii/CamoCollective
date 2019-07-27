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
        m_currentCharacter = character;
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
