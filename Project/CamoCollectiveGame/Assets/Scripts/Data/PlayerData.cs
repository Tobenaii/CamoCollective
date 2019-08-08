using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChonkJoustingData
{
    public int score;
    public int lives;
    public float respawnTimer;
    public bool isDead;
}

public class TowerClimbData
{
    public int placement;
    public float yPos;
    public bool isDead;
}

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
    [System.NonSerialized]
    private ChonkJoustingData m_chonkJoustingData = new ChonkJoustingData();
    [System.NonSerialized]
    private TowerClimbData m_towerClimbData = new TowerClimbData();

    public int RulerScore { get { return m_rulerScore; } set { m_rulerScore = value; } }
    public CharacterData Character { get { return m_currentCharacter; } set { m_currentCharacter = value; } }
    public bool IsPlaying { get { return m_isPlaying; } set { m_isPlaying = value; } }
    public int PlayerNum { get { return m_playerNum; } private set { } }
    public ChonkJoustingData ChonkJoustingData { get { return m_chonkJoustingData; } private set { } }
    public TowerClimbData TowerClimbData { get { return m_towerClimbData; } private set { } }

    public void RemoveCharacter()
    {
        if (m_currentCharacter == null)
            return;
        m_currentCharacter.inUse = false;
        m_currentCharacter = null;
    }
}
