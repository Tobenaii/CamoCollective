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
    private float m_prevTrigger;
    private bool m_initialized;

    private void Init()
    {
        m_characterPool.GetNextCharacter(m_playerData);
        m_initialized = true;
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
            m_characterPool.ReturnCharacter(m_playerData);
            m_initialized = false;
        }
        m_image.sprite = m_playerData.GetCharacter()?.GetIcon();
    }

    public void GetNextCharacter(float trigger)
    {
        if (trigger > 0 && m_prevTrigger == 0)
            m_characterPool.GetNextCharacter(m_playerData);
        m_prevTrigger = trigger;
    }
}
