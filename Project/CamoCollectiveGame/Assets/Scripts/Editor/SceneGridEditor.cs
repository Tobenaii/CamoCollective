using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SceneGrid))]
public class SceneGridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SceneGrid sceneGrid = (SceneGrid)target;
        if (GUILayout.Button("Load Scenes"))
            sceneGrid.LoadScenes();
        DrawDefaultInspector();
    }
}
