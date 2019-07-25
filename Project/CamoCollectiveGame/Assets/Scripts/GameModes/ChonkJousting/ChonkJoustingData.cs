using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChonkJoustingData : MonoBehaviour
{
    [SerializeField]
    private int m_spawnLives;
    private ChonkJouster m_jouster;

    public void SetChonkJouster(ChonkJouster jouster)
    {
        GetComponent<InputMapper>().SetControllerNum(jouster.player.GetPlayerNum());
        m_jouster = jouster;
        m_jouster.lives = m_spawnLives;
    }

    public void ResetLives()
    {
        m_jouster.lives = m_spawnLives;
    }

    public void RemoveLives(int value)
    {
        m_jouster.lives -= value;
    }

    public void AddLives(int value)
    {
        m_jouster.lives += value;
    }

    public int GetLives()
    {
        return m_jouster.lives;
    }

    public void RemoveScore(int value)
    {
        m_jouster.score -= value;
        if (m_jouster.score < 0)
            m_jouster.score = 0;
    }

    public void AddScore(int value)
    {
       m_jouster.score += value;
    }
}
