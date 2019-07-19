using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SceneValue))]
public class SceneValueProperty : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        var sceneRect = new Rect(position.x, position.y, position.width, position.height);
        EditorGUI.PropertyField(sceneRect, property.FindPropertyRelative("m_scene"), GUIContent.none);
        EditorGUI.EndProperty();


        var scene = ((SceneAsset)property.FindPropertyRelative("m_scene").objectReferenceValue);
        if (scene != null)
        {
            property.FindPropertyRelative("m_sceneName").stringValue = scene.name;
            property.serializedObject?.ApplyModifiedProperties();
        }
    }
}
