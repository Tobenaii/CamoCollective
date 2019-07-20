using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChonkJoustingData : MonoBehaviour
{
    [SerializeField]
    private int m_spawnLives;
    private int m_index;
    private ChonkJouster m_jouster;

    public void SetIndex(int index)
    {
        m_index = index;
    }

    public void SetChonkJouster(ChonkJouster jouster)
    {
        GetComponent<InputMapper>().SetControllerNum(jouster.player.GetPlayerNum() - 1);
        m_jouster = jouster;
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
    }

    public void AddScore(int value)
    {
       m_jouster.score += value;
    }
}
