using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChonkJoustingUI : MonoBehaviour
{
    [SerializeField]
    private PlayerData m_playerData;
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
        if (!m_playerData.IsPlaying)
        {
            gameObject.SetActive(false);
            return;
        }
        m_image.sprite = m_playerData.Character.Icon;
    }

    private void Update()
    {
        m_scoreText.text = m_playerData.ChonkJoustingData.score.ToString();
        m_lifeCounter.SetLivesValue(m_playerData.ChonkJoustingData.lives);
        if (m_playerData.ChonkJoustingData.isDead)
        {
            m_respawnPanel.SetActive(true);
            m_respawnText.text = ((int)(m_playerData.ChonkJoustingData.respawnTimer)).ToString();
        }
        else
            m_respawnPanel.SetActive(false);
    }
}
