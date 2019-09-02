﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionButton : MonoBehaviour
{
    [SerializeField]
    private GameObjectEvent m_hoverEvent;
    [SerializeField]
    private GameEvent m_clickEvent;
    [SerializeField]
    private GameObject m_targetGameObject;
    [SerializeField]
    private string m_name;
    [SerializeField]
    private Text m_text;
    [SerializeField]
    private FloatValue m_currentSelectedValue;
    [SerializeField]
    private int m_index;
    private bool m_invoked;

    private void Start()
    {
        if (m_currentSelectedValue.Value == m_index)
            SetHover();
    }

    private void Update()
    {
        if (EventSystem.current == null)
            return;
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            if (!m_invoked)
            {
                m_text.text = m_name;
                m_invoked = true;
                m_hoverEvent.Invoke(m_targetGameObject);
                m_currentSelectedValue.Value = m_index;
            }
        }
        else if (EventSystem.current.currentSelectedGameObject != gameObject)
            m_invoked = false;
    }

    public void SetHover()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnClick()
    {
        m_clickEvent.Invoke();
    }
}
