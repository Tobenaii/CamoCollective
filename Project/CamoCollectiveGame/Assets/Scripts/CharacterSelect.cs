using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

public class CharacterSelect : MonoBehaviour
{
    [SerializeField]
    private Image m_image;
    [SerializeField]
    private Text m_scoreText;
    [SerializeField]
    private PlayerData m_playerData;
    [SerializeField]
    private CharacterPool m_characterPool;
    [SerializeField]
    private GameObject m_joinPanel;
    private float m_prevTriggerNext;
    private float m_prevTriggerPrev;
    private bool m_initialized;

    private void Init()
    {
        m_characterPool.InitPlayer(m_playerData);
        m_initialized = true;
        m_image.gameObject.SetActive(true);
        m_playerData.SetPlaying(true);
        m_scoreText.text = m_playerData.GetRulerScore().ToString();
        m_joinPanel.SetActive(false);
    }

    private void Update()
    {
        GamePadState state = GamePad.GetState((PlayerIndex)m_playerData.GetPlayerNum() - 1);
        if (!state.IsConnected)
        {
            Disconnect();
        }
        m_image.sprite = m_playerData.GetCharacter()?.Icon;
        m_image.SetNativeSize();
    }

    public void Connect()
    {
        if (!m_initialized)
            Init();
    }

    public void Disconnect()
    {
        m_playerData.SetPlaying(false);
        m_characterPool.Disconnect(m_playerData);
        m_image.gameObject.SetActive(false);
        m_initialized = false;
        m_joinPanel.SetActive(true);
    }

    public void GetNextCharacter(float trigger)
    {
        if (trigger > 0 && m_prevTriggerNext == 0)
            m_characterPool.GetNextCharacter(m_playerData);
        m_prevTriggerNext = trigger;
    }

    public void GetPreviousCharacter(float trigger)
    {
        if (trigger > 0 && m_prevTriggerPrev == 0)
            m_characterPool.GetPreviousCharacter(m_playerData);
        m_prevTriggerPrev = trigger;
    }
}
