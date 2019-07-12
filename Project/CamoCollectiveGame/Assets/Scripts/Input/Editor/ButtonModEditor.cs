using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InputMapper.ButtonMod))]
public class ButtonModEditor : Editor
{
    SerializedProperty mod;

    private void OnEnable()
    {
    }

    public override void OnInspectorGUI()
    {
        Debug.Log("MERP");
        base.OnInspectorGUI();
    }
}
