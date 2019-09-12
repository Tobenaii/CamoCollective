using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class InputWindow : EditorWindow
{
    InputMapper.InputAction.Button currentButton = InputMapper.InputAction.Button.A;

    static Texture texture;
    static Texture2D circle;
    static Texture2D circleHover;
    static Texture2D oval;
    static Texture2D ovalHover;
    static GUIStyle style;

    private SerializedObject inputDataObj;
    private List<SerializedProperty> actionProps = new List<SerializedProperty>();
    private List<SerializedProperty> modProps = new List<SerializedProperty>();

    private bool canDisplay;
    private InputMapper input;

    private int topToolBar;
    private int botToolBar;
    private bool foldout;

    private Vector2 scroll;

    [MenuItem("Input/Controller Mapping")]
    static void Init()
    {
        var window = GetWindow<InputWindow>("Controller Mapping");
        window.Show();
    }

    private void OnFocus()
    {
        texture = Resources.Load("Controller") as Texture;
        circle = Resources.Load("Circle") as Texture2D;
        circleHover = Resources.Load("CircleHover") as Texture2D;
        oval = Resources.Load("Trigger") as Texture2D;
        ovalHover = Resources.Load("TriggerHover") as Texture2D;
        style = new GUIStyle();
    }

    private void OnSelectionChange()
    {
        UpdateButton();
    }

    private void UpdateButton()
    {
        if (Selection.gameObjects.Length <= 0 || !Selection.gameObjects[0]?.GetComponent<InputMapper>())
        {
            canDisplay = false;
            input = null;
            return;
        }
        input = Selection.gameObjects[0]?.GetComponent<InputMapper>();
        inputDataObj = new SerializedObject(input);
        actionProps.Clear();
        modProps.Clear();
        int i = 0;
        int c = 0;
        foreach (InputMapper.InputAction action in input.actions)
        {
            int b = 0;
            foreach (InputMapper.ButtonMod button in action.buttons)
            {
                if (button.button == currentButton)
                {
                    actionProps.Add(inputDataObj.FindProperty("actions").GetArrayElementAtIndex(c));
                    modProps.Add(actionProps[i].FindPropertyRelative("buttons").GetArrayElementAtIndex(b));
                    i++;
                }
                b++;
            }
            c++;
        }
        canDisplay = true;
    }

    private void Update()
    {
        Repaint();
    }

    private void OnGUI()
    {
        UpdateButton();
        if (input == null)
        {
            GUILayout.Label("The game object must have an InputMapper component attached");
            return;
        }
        scroll = EditorGUILayout.BeginScrollView(scroll);
        EditorGUI.indentLevel = 0;
        topToolBar = GUILayout.Toolbar(topToolBar, new string[] { "Controller", "Actions" });
        if (topToolBar == 1)
        {
            EditorGUILayout.PropertyField(inputDataObj.FindProperty("actions"), true);
            //foreach (InputMapper.InputAction action in input.actions)
            //{
            //    EditorGUILayout.Foldout(false, action.actionName);
            //}
            inputDataObj.ApplyModifiedProperties();
            EditorGUILayout.EndScrollView();
            return;
        }

        GUILayout.Box(texture);

        DrawButton(new Rect(360, 230, 40, 40), circle, circleHover, style, InputMapper.InputAction.Button.A);
        DrawButton(new Rect(330, 200, 40, 40), circle, circleHover, style, InputMapper.InputAction.Button.X);
        DrawButton(new Rect(360, 170, 40, 40), circle, circleHover, style, InputMapper.InputAction.Button.Y);
        DrawButton(new Rect(392, 200, 40, 40), circle, circleHover, style, InputMapper.InputAction.Button.B);
        DrawButton(new Rect(270, 200, 40, 40), circle, circleHover, style, InputMapper.InputAction.Button.Start);

        DrawButton(new Rect(117, 195, 55, 55), circle, circleHover, style, InputMapper.InputAction.Button.LeftJoystick);
        DrawButton(new Rect(293, 263, 55, 55), circle, circleHover, style, InputMapper.InputAction.Button.RightJoystick);

        DrawButton(new Rect(117, 90, 55, 55), oval, ovalHover, style, InputMapper.InputAction.Button.LeftTrigger);
        DrawButton(new Rect(350, 90, 55, 55), oval, ovalHover, style, InputMapper.InputAction.Button.RightTrigger);

        if (canDisplay && actionProps.Count > 0)
        {
            botToolBar = GUILayout.Toolbar(botToolBar, new string[] { "Events", "Modifiers" });
            if (botToolBar == 0)
            {
                EditorGUILayout.LabelField("Button " + currentButton.ToString());
                for (int i = 0; i < actionProps.Count; i++)
                {
                    if (currentButton == InputMapper.InputAction.Button.LeftTrigger || currentButton == InputMapper.InputAction.Button.RightTrigger)
                        EditorGUILayout.PropertyField(actionProps[i].FindPropertyRelative("unityEventTrigger"), new GUIContent(actionProps[i].FindPropertyRelative("actionName").stringValue));
                    else if (currentButton == InputMapper.InputAction.Button.LeftJoystick || currentButton == InputMapper.InputAction.Button.RightJoystick)
                        EditorGUILayout.PropertyField(actionProps[i].FindPropertyRelative("unityEventJoystick"), new GUIContent(actionProps[i].FindPropertyRelative("actionName").stringValue));
                    else
                        EditorGUILayout.PropertyField(actionProps[i].FindPropertyRelative("unityEventButton"), new GUIContent(actionProps[i].FindPropertyRelative("actionName").stringValue));
                }
            }
            else if (currentButton != InputMapper.InputAction.Button.LeftJoystick && currentButton != InputMapper.InputAction.Button.RightJoystick && currentButton != InputMapper.InputAction.Button.LeftTrigger && currentButton != InputMapper.InputAction.Button.RightTrigger)
            {
                for (int i = 0; i < modProps.Count; i++)
                {
                    foldout = EditorGUILayout.Foldout(foldout, actionProps[i].FindPropertyRelative("actionName").stringValue);
                    if (foldout)
                    {
                        int mods = 0;
                        mods += Convert.ToInt32(DrawModifier(i, "delay", "delayTime"));
                        mods += Convert.ToInt32(DrawModifier(i, "cooldown", "cooldownTime"));
                        mods += Convert.ToInt32(DrawModifier(i, "onButtonDown"));
                        mods += Convert.ToInt32(DrawModifier(i, "onButtonHold"));
                        mods += Convert.ToInt32(DrawModifier(i, "onButtonRelease"));
                        if (mods > 1)
                            EditorGUILayout.LabelField("Caution! Multiple modifiers may cause this action to be invoked more than intended.");
                        else if (mods == 0)
                            EditorGUILayout.LabelField("Warning! No modifiers means no events will be invoked!");

                    }
                }
            }
            else
                EditorGUILayout.LabelField("Sorry, there are no modifiers for this action at this time.");
        }
        else
            EditorGUILayout.LabelField("This button has not been assigned to any actions.");
        inputDataObj.ApplyModifiedProperties();
        EditorGUILayout.EndScrollView();
    }

    private bool DrawModifier(int index, string modBool, string modValue = "")
    {
        EditorGUI.indentLevel = 1;
        SerializedProperty delay = modProps[index].FindPropertyRelative(modBool);
        EditorGUILayout.PropertyField(delay);
        if (delay.boolValue && modValue != "")
        {
            EditorGUI.indentLevel = 2;
            EditorGUILayout.PropertyField(modProps[index].FindPropertyRelative(modValue));
            EditorGUI.indentLevel = 1;
        }
        return delay.boolValue;
    }

    private void DrawButton(Rect rect, Texture2D sprite, Texture2D spriteSelected, GUIStyle guiStyle, InputMapper.InputAction.Button button)
    {
        if (GUI.Button(rect, sprite, guiStyle))
        {
            currentButton = button;
        }
        if (currentButton == button)
            GUI.Box(rect, spriteSelected);
    }
}
