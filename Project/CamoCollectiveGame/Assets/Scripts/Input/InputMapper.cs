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
    public class TriggerEvent : UnityEvent<float>
    {
    }

    [System.Serializable]
    public class InputAction
    {
        public enum Button { A, B, Y, X, Start, LeftJoystick, RightJoystick, LeftTrigger, RightTrigger, LeftBumper, RightBumper};

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
            buttonStart = false;
            joystickLeft = false;
            joystickRight = false;
            triggerLeft = false;
            triggerRight = false;
            leftBumper = false;
            rightBumper = false;
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
                    unityEventJoystick.Invoke(new Vector2((state.DPad.Left == ButtonState.Pressed)?-1:(state.DPad.Right == ButtonState.Pressed)?1:state.ThumbSticks.Left.X, (state.DPad.Down == ButtonState.Pressed) ? -1 : (state.DPad.Up == ButtonState.Pressed) ? 1 : state.ThumbSticks.Left.Y));
                if (mod.button == InputAction.Button.LeftTrigger)
                    unityEventTrigger.Invoke(state.Triggers.Left);
                if (mod.button == InputAction.Button.RightTrigger)
                    unityEventTrigger.Invoke(state.Triggers.Right);
                mod.Update(unityEventButton);
            }
        }

        public string actionName;
        public bool buttonA;
        public bool buttonB;
        public bool buttonX;
        public bool buttonY;
        public bool buttonStart;
        public bool joystickLeft;
        public bool joystickRight;
        public bool triggerLeft;
        public bool triggerRight;
        public bool leftBumper;
        public bool rightBumper;
        [HideInInspector]
        public List<ButtonMod> buttons = new List<ButtonMod>();
        [HideInInspector]
        [SerializeField]
        public UnityEvent unityEventButton = new UnityEvent();
        [HideInInspector]
        [SerializeField]
        public JoystickEvent unityEventJoystick = new JoystickEvent();
        [HideInInspector]
        [SerializeField]
        public TriggerEvent unityEventTrigger = new TriggerEvent();
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
            {
                Invoke(unityEvent);
            }
            if (onButtonHold && buttonState == ButtonState.Pressed)
                Invoke(unityEvent);
            if (onButtonRelease && buttonState == ButtonState.Released && prevButtonState == ButtonState.Pressed)
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

    [SerializeField]
    private bool m_disableOnAwake;
    [SerializeField]
    private FloatReference m_controllerNumber;
    public List<InputAction> actions = new List<InputAction>();
    private GamePadState state;
    private GamePadState prevState;
    private bool m_disabled;

    private void Awake()
    {
        m_disabled = m_disableOnAwake;
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
            if (action.buttonStart)
                AddButton(action, InputAction.Button.Start);
            else
                RemoveButton(action, InputAction.Button.Start);
            if (action.joystickLeft)
                AddButton(action, InputAction.Button.LeftJoystick);
            else
                RemoveButton(action, InputAction.Button.LeftJoystick);
            if (action.joystickRight)
                AddButton(action, InputAction.Button.RightJoystick);
            else
                RemoveButton(action, InputAction.Button.RightJoystick);
            if (action.triggerLeft)
                AddButton(action, InputAction.Button.LeftTrigger);
            else
                RemoveButton(action, InputAction.Button.LeftTrigger);
            if (action.triggerRight)
                AddButton(action, InputAction.Button.RightTrigger);
            else
                RemoveButton(action, InputAction.Button.RightTrigger);
            if (action.leftBumper)
                AddButton(action, InputAction.Button.LeftBumper);
            else
                RemoveButton(action, InputAction.Button.LeftBumper);
            if (action.rightBumper)
                AddButton(action, InputAction.Button.RightBumper);
            else
                RemoveButton(action, InputAction.Button.RightBumper);
        }
    }

    public void DisableInput()
    {
        m_disabled = true;
    }

    public void EnableInput()
    {
        m_disabled = false;
    }

    public void SetToControllerNum(FloatValue value)
    {
        value.Value = m_controllerNumber.Value;
    }

    private void OnDestroy()
    {
        GamePad.SetVibration((PlayerIndex)m_controllerNumber.Value - 1, 0, 0);
    }

    public void Vibrate(float seconds, float leftMotor, float rightMotor)
    {
        StopCoroutine(VibrateForSeconds(0));
        GamePad.SetVibration((PlayerIndex)m_controllerNumber.Value - 1, leftMotor, rightMotor);
        StartCoroutine(VibrateForSeconds(seconds));
    }

    private IEnumerator VibrateForSeconds(float seconds)
    {
        float timer = seconds;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        GamePad.SetVibration((PlayerIndex)m_controllerNumber.Value - 1, 0, 0);
    }

    private void Update()
    {
        if (m_disabled)
            return;
        prevState = state;
        state = GamePad.GetState((PlayerIndex)m_controllerNumber.Value - 1, GamePadDeadZone.Circular);
        //if (state.Buttons.X == ButtonState.Pressed)
        //    GamePad.SetVibration((PlayerIndex)m_controllerNumber.Value - 1, 1, 1);
        //else
        //    GamePad.SetVibration((PlayerIndex)m_controllerNumber.Value - 1, 0, 0);
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
                    case InputAction.Button.Start:
                        button.buttonState = state.Buttons.Start;
                        button.prevButtonState = prevState.Buttons.Start;
                        break;
                    case InputAction.Button.LeftBumper:
                        button.buttonState = state.Buttons.LeftShoulder;
                        button.prevButtonState = prevState.Buttons.LeftShoulder;
                        break;
                    case InputAction.Button.RightBumper:
                        button.buttonState = state.Buttons.RightShoulder;
                        button.prevButtonState = prevState.Buttons.RightShoulder;
                        break;
                }
            }
            action.Update(state);
        }
    }
}
