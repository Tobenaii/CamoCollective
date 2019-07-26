using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChonkJoustingData : MonoBehaviour
{
    [SerializeField]
    private int m_spawnLives;
    private ChonkJouster m_jouster;
    private int m_score;
    private int m_lives;

    public void ResetScore()
    {
        m_jouster.score = 0;
    }

    public void SetChonkJouster(ChonkJouster jouster)
    {
        GetComponent<InputMapper>().SetControllerNum(jouster.player.GetPlayerNum());
        m_jouster = jouster;
        ResetLives();
        ResetScore();
    }

    public void ResetLives()
    {
        if (m_jouster == null)
            return;
        m_jouster.lives = m_spawnLives;
    }

    public void RemoveLives(int value)
    {
        if (m_jouster == null)
            return;
        m_jouster.lives -= value;
    }

    public void AddLives(int value)
    {
        if (m_jouster == null)
            return;
        m_jouster.lives += value;
    }

    public int GetLives()
    {
        if (m_jouster == null)
            return 0;
        return m_jouster.lives;
    }

    public void SetDead(bool dead)
    {
        if (m_jouster == null)
            return;
        m_jouster.isDead = dead;
    }

    public void RemoveScore(int value)
    {
        if (m_jouster == null)
            return;
        m_jouster.score -= value;
        if (m_jouster.score < 0)
            m_jouster.score = 0;
    }

    public void AddScore(int value)
    {
        if (m_jouster == null)
            return;
        m_jouster.score += value;
    }

    public void SetRespawnTimer(float timer)
    {
        if (m_jouster == null)
            return;
        m_jouster.respawnTimer = timer;
    }
}
