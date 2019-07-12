using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using XInputDotNetPure;

[System.Serializable]
public class InputMapper : MonoBehaviour
{
    [System.Serializable]
    public class JoystickEvent : UnityEvent<Vector2>
    {
    }

    [System.Serializable]
    public class InputAction
    {
        public enum Button { A, B, Y, X , LeftJoystick, RightJoystick};

        public static implicit operator InputAction(string action)
        {
            return new InputAction(action);
        }

        public override int GetHashCode()
        {
            return actionName.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return CheckEquals((InputAction)obj);
        }

        public bool CheckEquals(InputAction action)
        {
            return (action != null && action.actionName == actionName);
        }

        public InputAction()
        {
            actionName = "New Action";
            buttonA = false;
            buttonB = false;
            buttonX = false;
            buttonY = false;
            joystickLeft = false;
            joystickRight = false;
            buttons.Clear();
            unityEventButton = new UnityEvent();
            unityEventJoystick = new JoystickEvent();
        }
        public InputAction(string action)
        {
            actionName = action;
        }

        public void Update(GamePadState state)
        {
            foreach (ButtonMod mod in buttons)
            {
                if (mod.button == InputAction.Button.RightJoystick)
                    unityEventJoystick.Invoke(new Vector2(state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y));
                if (mod.button == InputAction.Button.LeftJoystick)
                    unityEventJoystick.Invoke(new Vector2(state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y));
                else
                    mod.Update(unityEventButton);
            }
        }

        public string actionName;
        public bool buttonA;
        public bool buttonB;
        public bool buttonX;
        public bool buttonY;
        public bool joystickLeft;
        public bool joystickRight;
        [HideInInspector]
        public List<ButtonMod> buttons = new List<ButtonMod>();
        [HideInInspector]
        public UnityEvent unityEventButton = new UnityEvent();
        [HideInInspector]
        public JoystickEvent unityEventJoystick = new JoystickEvent();
    }


    [System.Serializable]
    public class ButtonMod
    {
        public InputAction.Button button;
        [HideInInspector]
        public ButtonState buttonState;
        [HideInInspector]
        public ButtonState prevButtonState;

        [HideInInspector]
        public bool delay;
        [HideInInspector]
        public bool cooldown;
        [HideInInspector]
        public bool onButtonDown;
        [HideInInspector]
        public bool onButtonHold;
        [HideInInspector]
        public bool onButtonRelease;

        private float cooldownTimer;
        private float delayTimer;

        public void Update(UnityEvent unityEvent)
        {
            CheckButton(unityEvent);
        }

        private void CheckButton(UnityEvent unityEvent)
        {
            if (onButtonDown && buttonState == ButtonState.Pressed && prevButtonState == ButtonState.Released)
                Invoke(unityEvent);
            if (onButtonHold && buttonState == ButtonState.Pressed)
                Invoke(unityEvent);
            if (onButtonRelease && prevButtonState == ButtonState.Released && prevButtonState == ButtonState.Pressed)
                Invoke(unityEvent);
        }

        private void Invoke(UnityEvent unityEvent)
        {
            unityEvent.Invoke();
        }

        //Delay props
        [HideInInspector]
        public float delayTime;

        //Cooldown props
        [HideInInspector]
        public float cooldownTime;
    }


    public List<InputAction> actions;

    [SerializeField]
    private int controllerNum;
    private GamePadState state;
    private GamePadState prevState;

    private void Awake()
    {
    }

    private void AddButton(InputAction action, InputAction.Button button)
    {
        bool hasButton = false;
        foreach (ButtonMod b in action.buttons)
        {
            if (b.button == button)
            {
                hasButton = true;
                return;
            }
        }
        if (hasButton)
            return;
        ButtonMod mod = new ButtonMod();
        mod.button = button;
        action.buttons.Add(mod);
    }

    private void RemoveButton(InputAction action, InputAction.Button button)
    {
        ButtonMod mod = null;
        foreach (ButtonMod b in action.buttons)
        {
            if (b.button == button)
            {
                mod = b;
                break;
            }
        }
        if (mod == null)
            return;
        action.buttons.Remove(mod);
    }

    private void OnValidate()
    {
        foreach (InputAction action in actions)
        {
            if (action.buttonA)
                AddButton(action, InputAction.Button.A);
            else
                RemoveButton(action, InputAction.Button.A);
            if (action.buttonB)
                AddButton(action, InputAction.Button.B);
            else
                RemoveButton(action, InputAction.Button.B);
            if (action.buttonX)
                AddButton(action, InputAction.Button.X);
            else
                RemoveButton(action, InputAction.Button.X);
            if (action.buttonY)
                AddButton(action, InputAction.Button.Y);
            else
                RemoveButton(action, InputAction.Button.Y);
            if (action.joystickLeft)
                AddButton(action, InputAction.Button.LeftJoystick);
            else
                RemoveButton(action, InputAction.Button.LeftJoystick);
            if (action.joystickRight)
                AddButton(action, InputAction.Button.RightJoystick);
            else
                RemoveButton(action, InputAction.Button.RightJoystick);
        }
    }

    private void Update()
    {
        prevState = state;
        state = GamePad.GetState((PlayerIndex)controllerNum);
        foreach (InputAction action in actions)
        {
            foreach (ButtonMod button in action.buttons)
            {
                switch (button.button)
                {
                    case InputAction.Button.A:
                        button.buttonState = state.Buttons.A;
                        button.prevButtonState = prevState.Buttons.A;
                        break;
                    case InputAction.Button.X:
                        button.buttonState = state.Buttons.X;
                        button.prevButtonState = prevState.Buttons.X;
                        break;
                    case InputAction.Button.B:
                        button.buttonState = state.Buttons.B;
                        button.prevButtonState = prevState.Buttons.B;
                        break;
                    case InputAction.Button.Y:
                        button.buttonState = state.Buttons.Y;
                        button.prevButtonState = prevState.Buttons.Y;
                        break;
                }
            }
            action.Update(state);
        }
    }
}
