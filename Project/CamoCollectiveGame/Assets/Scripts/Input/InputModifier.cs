using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using XInputDotNetPure;

[System.Serializable]
[CreateAssetMenu(menuName = "Input/Modifier")]
public class InputModifier : ScriptableObject
{
    private GamePadState state;

    public void Update()
    {
        state = GamePad.GetState(0);
        if (state.Buttons.X == ButtonState.Pressed)
        {

        }

        if (state.Buttons.A == ButtonState.Pressed)
        {

        }
    }
}
