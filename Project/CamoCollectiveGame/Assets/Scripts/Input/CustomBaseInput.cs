using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XInputDotNetPure;

public class CustomBaseInput : BaseInput
{
    [SerializeField]
    private FloatValue m_controllerNumber;
    GamePadState state;
    GamePadState prevState;

    private void Update()
    {
        prevState = state;
        state = GamePad.GetState((PlayerIndex)m_controllerNumber.value - 1);
    }

    public override float GetAxisRaw(string axisName)
    {
        string[] inputs = axisName.Split('.');
        switch (inputs[0])
        {
            case "LeftJoystick":
                if (inputs[1] == "X")
                    return state.ThumbSticks.Left.X;
                else if (inputs[1] == "Y")
                    return state.ThumbSticks.Left.Y;
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
        state = GamePad.GetState((PlayerIndex)m_controllerNumber.value - 1);
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
        return false;
    }

    public override bool GetMouseButtonUp(int button)
    {
        return false;
    }

    public override Vector2 mousePosition => Vector3.zero;
}
