using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class ReferenceEditor : PropertyDrawer
{
    

    private void OnEnable()
    {
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        SerializedProperty useConstant = property.FindPropertyRelative("useConstant");

        var constantRect = new Rect(position.x, position.y, 30, position.height);
        var valueRect = new Rect(position.x + 30, position.y, 50, position.height);

        EditorGUI.PropertyField(constantRect, useConstant, GUIContent.none);

        if ((useConstant.boolValue))
            EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("m_constant"), GUIContent.none);
        else
            EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("m_variable"), GUIContent.none);

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}
