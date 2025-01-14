﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

public class CharacterSelect : MonoBehaviour
{
    [SerializeField]
    private GameObject m_panel;
    [SerializeField]
    private Image m_image;
    [SerializeField]
    private PlayerData m_playerData;
    [SerializeField]
    private List<CharacterData> m_characterPool;
    [SerializeField]
    private GameObject m_joinText;
    [SerializeField]
    private GameObject m_playerNumberText;
    [SerializeField]
    private Vector3Reference m_cursorPos;

    private float m_prevTriggerNext;
    private float m_prevTriggerPrev;
    private bool m_initialized;
    private int m_currentIndex;

    private void Awake()
    {
        m_joinText.SetActive(true);
        m_playerNumberText.SetActive(false);
        if (m_playerData.IsPlaying)
            Connect();
    }

    public void PlayCheck()
    {
        if (!m_playerData.IsPlaying)
            m_panel.SetActive(false);
    }

    public void Show()
    {
        m_panel.SetActive(true);
    }

    private void Init()
    {
        //m_currentIndex = -1;
        //if (m_playerData.Character == null)
        //    GetNextCharacter(-1);

        //m_initialized = true;
        //m_image.gameObject.SetActive(true);
        //m_playerData.IsPlaying = true;
        //m_joinText.SetActive(false);
        //m_playerNumberText.SetActive(true);

        m_initialized = true;
        m_playerData.IsPlaying = true;
        m_joinText.SetActive(false);
        m_playerNumberText.SetActive(true);
        m_image.gameObject.SetActive(true);
    }

    //private void GetNextCharacter(int index)
    //{
    //    index++;
    //    if (index == m_characterPool.Count)
    //        index = 0;
    //    if (index == m_currentIndex)
    //        return;
    //    if (TryChangeCharacter(index))
    //        return;
    //    else
    //        GetNextCharacter(index);
    //}

    //private void GetPreviousCharacter(int index)
    //{
    //    index--;
    //    if (index < 0)
    //        index = m_characterPool.Count - 1;
    //    if (index == m_currentIndex)
    //        return;
    //    if (TryChangeCharacter(index))
    //        return;
    //    else
    //        GetPreviousCharacter(index);
    //}

    //private bool TryChangeCharacter(int index)
    //{
    //    if (!m_characterPool[index].inUse)
    //    {
    //        if (m_playerData.Character != null)
    //            m_playerData.Character.inUse = false;
    //        m_playerData.Character = m_characterPool[index];
    //        m_currentIndex = index;
    //        m_playerData.Character.inUse = true;
    //        return true;
    //    }
    //    return false;
    //}

    private void Update()
    {
        if (!m_initialized && m_playerData.IsPlaying)
            Init();
        else if (m_initialized && !m_playerData.IsPlaying)
            Disconnect();
        if (m_playerData.Character == null)
        {
            if (m_playerData.GhostCharacter == null)
                m_image.color = new Color(0, 0, 0, 0);
            else
            {
                Color c = m_playerData.IndicatorColour;
                m_image.color = new Color(1, 1, 1, 0.75f);
                m_image.sprite = m_playerData.GhostCharacter.Icon;
            }
        }
        else
        {
            m_image.color = new Color(1, 1, 1, 1);
            m_image.sprite = m_playerData.Character.Icon;
        }

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
        m_playerData.IsPlaying = false;
        m_playerData.RemoveCharacter();
        m_image.gameObject.SetActive(false);
        m_initialized = false;
        m_joinText.SetActive(true);
        m_playerNumberText.SetActive(false);
    }

    //public void GetNextCharacter(float trigger)
    //{
    //    if (trigger > 0 && m_prevTriggerNext == 0)
    //        GetNextCharacter(m_currentIndex);
    //    m_prevTriggerNext = trigger;
    //}

    //public void GetPreviousCharacter(float trigger)
    //{
    //    if (trigger > 0 && m_prevTriggerPrev == 0)
    //        GetPreviousCharacter(m_currentIndex);
    //    m_prevTriggerPrev = trigger;
    //}
}
