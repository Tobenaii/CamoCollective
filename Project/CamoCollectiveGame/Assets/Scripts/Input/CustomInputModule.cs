using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CustomBaseInput))]
public class CustomInputModule : StandaloneInputModule
{
    protected override void Awake()
    {
        base.m_InputOverride = gameObject.GetComponent<CustomBaseInput>();
    }
}
