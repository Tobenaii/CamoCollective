using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChonkJoustingUI : MonoBehaviour
{
    [SerializeField]
    private ChonkJouster m_chonkJouster;
    [SerializeField]
    private Image m_image;
    [SerializeField]
    private Text m_scoreText;
    [SerializeField]
    private LifeCounter m_lifeCounter;
    [SerializeField]
    private GameObject m_respawnPanel;
    [SerializeField]
    private Text m_respawnText;

    private void Start()
    {
        if (m_chonkJouster.player == null)
        {
            gameObject.SetActive(false);
            return;
        }
        m_image.sprite = m_chonkJouster.player.GetCharacter().GetIcon();
    }

    private void Update()
    {
        m_scoreText.text = m_chonkJouster.score.ToString();
        m_lifeCounter.SetLivesValue(m_chonkJouster.lives);
        if (m_chonkJouster.isDead)
        {
            m_respawnPanel.SetActive(true);
            m_respawnText.text = ((int)(m_chonkJouster.respawnTimer)).ToString();
        }
        else
            m_respawnPanel.SetActive(false);
        //m_scoreText.text = m_scores[m_index].ToString();
        //m_lifeCounter.SetLivesValue(m_lives[m_index]);
    }
}
