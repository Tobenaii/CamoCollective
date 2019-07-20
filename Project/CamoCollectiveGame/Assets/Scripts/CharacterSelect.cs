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
    private PlayerData m_playerData;
    [SerializeField]
    private CharacterPool m_characterPool;
    private float m_prevTriggerNext;
    private float m_prevTriggerPrev;
    private bool m_initialized;

    private void Init()
    {
        m_characterPool.GetNextCharacter(m_playerData);
        m_initialized = true;
        m_image.enabled = true;
    }

    private void Update()
    {
        GamePadState state = GamePad.GetState((PlayerIndex)m_playerData.GetPlayerNum() - 1);
        if (state.IsConnected)
        {
            if (!m_initialized)
                Init();
            m_playerData.SetPlaying(true);
        }
        else
        {
            m_playerData.SetPlaying(false);
            m_image.enabled = false;
            m_characterPool.ReturnCharacter(m_playerData);
            m_initialized = false;
        }
        m_image.sprite = m_playerData.GetCharacter()?.GetIcon();
        m_image.SetNativeSize();
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
