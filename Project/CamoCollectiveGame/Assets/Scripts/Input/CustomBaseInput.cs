using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XInputDotNetPure;

public class CustomBaseInput : BaseInput
{
    [SerializeField]
    private bool m_disableMouse;
    [SerializeField]
    private BoolValue m_isInputDisabled;
    [SerializeField]
    private PlayerData m_playerData;
    [SerializeField]
    private FloatReference m_controllerNumber;
    [SerializeField]
    private Vector3Reference m_cursorPosValue;
    [SerializeField]
    private float m_cursorMoveSpeed;
    private GamePadState state;
    private GamePadState prevState;
    private Vector2 m_cursorPos;
    private bool m_firstUpdate;

    public PlayerData Player {get { return m_playerData; } private set { } }

    protected override void Start()
    {
        
    }

    private void Update()
    {
        Debug.Log(m_isInputDisabled.Value);
        if (m_isInputDisabled.Value)
            return;
        if (m_firstUpdate && m_cursorPos != (Vector2)m_cursorPosValue.Value)
            m_cursorPos = m_cursorPosValue.Value;
        if (!m_firstUpdate)
        {
            m_cursorPos = m_cursorPosValue.Value;
            m_firstUpdate = true;
        }

        prevState = state;
        state = GamePad.GetState((PlayerIndex)m_controllerNumber.Value - 1, GamePadDeadZone.Circular);
        Vector2 dir = new Vector2(((state.DPad.Left == ButtonState.Pressed) ? -1 : ((state.DPad.Right == ButtonState.Pressed) ? 1 : state.ThumbSticks.Left.X)), ((state.DPad.Down == ButtonState.Pressed) ? -1 : ((state.DPad.Up == ButtonState.Pressed) ? 1 : state.ThumbSticks.Left.Y)));
        m_cursorPos += dir.normalized * Screen.width * m_cursorMoveSpeed * Time.deltaTime;
        if (m_cursorPos.x < 0)
            m_cursorPos.x = 0;
        if (m_cursorPos.y < 0)
            m_cursorPos.y = 0;
        if (m_cursorPos.x > Screen.width)
            m_cursorPos.x = Screen.width;
        if (m_cursorPos.y > Screen.height)
            m_cursorPos.y = Screen.height;

        m_cursorPosValue.Value = m_cursorPos;
    }

    public override float GetAxisRaw(string axisName)
    {
        if (m_isInputDisabled.Value)
            return 0;
        string[] inputs = axisName.Split('.');
        switch (inputs[0])
        {
            case "LeftJoystick":
                if (inputs[1] == "X")
                    return state.ThumbSticks.Left.X != 0?state.ThumbSticks.Left.X:state.DPad.Left == ButtonState.Pressed?-1:state.DPad.Right == ButtonState.Pressed?1:0;
                else if (inputs[1] == "Y")
                    return state.ThumbSticks.Left.Y != 0?state.ThumbSticks.Left.Y:state.DPad.Down == ButtonState.Pressed?-1:state.DPad.Up == ButtonState.Pressed?1:0;
                break;
            case "RightJoystick":
                if (inputs[1] == "X")
                    return state.ThumbSticks.Right.X;
                else if (inputs[1] == "Y")
                    return state.ThumbSticks.Right.Y;
                break;
        }
        return 0;
    }

    public override bool GetButtonDown(string buttonName)
    {
        if (m_isInputDisabled.Value || buttonName == "")
            return false;
        switch (buttonName)
        {
            case "ButtonA":
                return state.Buttons.A == ButtonState.Pressed && prevState.Buttons.A == ButtonState.Released;
            case "ButtonB":
                return state.Buttons.B == ButtonState.Pressed && prevState.Buttons.B == ButtonState.Released;
            case "ButtonX":
                return state.Buttons.X == ButtonState.Pressed && prevState.Buttons.X == ButtonState.Released;
            case "ButtonY":
                return state.Buttons.Y == ButtonState.Pressed && prevState.Buttons.Y == ButtonState.Released;
        }
        return false;
    }

    public override bool GetMouseButtonDown(int button)
    {
        if (m_isInputDisabled.Value || m_disableMouse)
            return false;
        return state.Buttons.A == ButtonState.Pressed && prevState.Buttons.A == ButtonState.Released;
    }

    public override bool GetMouseButtonUp(int button)
    {
        if (m_isInputDisabled.Value || m_disableMouse)
            return false;
        return state.Buttons.A == ButtonState.Released && prevState.Buttons.A == ButtonState.Pressed;
    }

    public override Vector2 mousePosition => m_cursorPos;
}
