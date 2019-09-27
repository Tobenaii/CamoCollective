using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CustomBaseInput))]
public class CustomInputModule : StandaloneInputModule
{
    private CustomBaseInput m_baseInput;
    public PlayerData Player {get { return m_baseInput.Player; } private set { } }
    public CustomBaseInput BaseInput => m_baseInput;

    protected override void Awake()
    {
        m_baseInput = gameObject.GetComponent<CustomBaseInput>();
        base.m_InputOverride = m_baseInput;
    }
}
