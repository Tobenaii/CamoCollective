using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class VariableProperty : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        SerializedObject so = new SerializedObject(property.objectReferenceValue);
        SerializedProperty indexProp = so.FindProperty("m_index");

        var valueRect = new Rect(position.x, position.y, 100, position.height);
        var indexRect = new Rect(position.x + 100, position.y, 50, position.height);

        EditorGUI.PropertyField(valueRect, property, GUIContent.none);
        EditorGUI.PropertyField(indexRect, indexProp, GUIContent.none);

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}
