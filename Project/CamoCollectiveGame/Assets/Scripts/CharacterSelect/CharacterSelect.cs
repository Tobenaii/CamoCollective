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
    private List<CharacterData> m_characterPool;
    [SerializeField]
    private GameObject m_joinPanel;
    [SerializeField]
    private GameEvent m_closeEvent;


    private float m_prevTriggerNext;
    private float m_prevTriggerPrev;
    private bool m_initialized;
    private int m_currentIndex;

    private void Awake()
    {
        if (m_playerData.IsPlaying())
            Connect();
    }

    private void Init()
    {
        m_currentIndex = -1;
        if (m_playerData.GetCharacter() == null)
            GetNextCharacter(-1);

        m_initialized = true;
        m_image.gameObject.SetActive(true);
        m_playerData.SetPlaying(true);
        m_scoreText.text = m_playerData.GetRulerScore().ToString();
        m_joinPanel.SetActive(false);
    }

    private void GetNextCharacter(int index)
    {
        if (m_characterPool.Count == 0)
            return;
        index++;
        if (index == m_characterPool.Count)
            index = 0;
        if (index == m_currentIndex)
            return;
        if (!m_characterPool[index].inUse)
        {
            m_playerData.SetCharacter(m_characterPool[index]);
            m_currentIndex = index;
            return;
        }
        else
            GetNextCharacter(index);
    }

    private void GetPreviousCharacter(int index)
    {
        if (m_characterPool.Count == 0)
            return;
        index--;
        if (index < 0)
            index = m_characterPool.Count - 1;
        if (index == m_currentIndex)
            return;
        if (!m_characterPool[index].inUse)
        {
            m_playerData.SetCharacter(m_characterPool[index]);
            m_currentIndex = index;
            return;
        }
        else
            GetNextCharacter(index);
    }

    private void Update()
    {
        GamePadState state = GamePad.GetState((PlayerIndex)m_playerData.GetPlayerNum() - 1);
        if (!state.IsConnected)
            Disconnect();

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
        m_currentIndex = 0;
        m_playerData.SetPlaying(false);
        m_playerData.RemoveCharacter();
        m_image.gameObject.SetActive(false);
        m_initialized = false;
    }

    public void GetNextCharacter(float trigger)
    {
        if (trigger > 0 && m_prevTriggerNext == 0)
            GetNextCharacter(m_currentIndex);
        m_prevTriggerNext = trigger;
    }

    public void GetPreviousCharacter(float trigger)
    {
        if (trigger > 0 && m_prevTriggerPrev == 0)
            GetPreviousCharacter(m_currentIndex);
        m_prevTriggerPrev = trigger;
    }

    public void OnCharacterSelectOpen()
    {
        if (!m_playerData.IsPlaying())
            m_joinPanel.SetActive(true);
    }

    public void OnCharacterSelectClose()
    {
        if (!m_playerData.IsPlaying())
            m_joinPanel.SetActive(false);
    }
}
