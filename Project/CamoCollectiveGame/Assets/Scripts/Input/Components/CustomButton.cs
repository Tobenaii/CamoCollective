using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : Button
{
    private ButtonEvent m_buttonEvent;
    private int m_hoverCount;
    private List<PlayerData> m_playersHovering = new List<PlayerData>();

    protected override void Awake()
    {
        m_buttonEvent = GetComponent<ButtonEvent>();
        base.Awake();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (!interactable)
            return;
        base.OnPointerEnter(eventData);
        m_hoverCount++;
        PlayerData player = (eventData.currentInputModule as CustomInputModule).Player;
        m_buttonEvent.OnCursorEnter.Invoke(player);
        m_playersHovering.Add(player);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (!interactable)
            return;
        m_hoverCount--;
        if (m_hoverCount == 0)
            base.OnPointerExit(eventData);
        PlayerData player = (eventData.currentInputModule as CustomInputModule).Player;
        m_buttonEvent.OnCursorExit.Invoke(player);
        m_playersHovering.Remove(player);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (!interactable)
            return;
        base.OnPointerClick(eventData);
        m_buttonEvent.OnCursorClick.Invoke((eventData.currentInputModule as CustomInputModule).Player);
        for (int i = 0; i < m_hoverCount; i++)
            OnPointerExit(null);
        foreach (PlayerData player in m_playersHovering)
            m_buttonEvent.OnCursorExit.Invoke(player);
        interactable = false;
    }
}
