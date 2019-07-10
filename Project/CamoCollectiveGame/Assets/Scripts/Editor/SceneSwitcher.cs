using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher: EditorWindow
{
    private static int m_buttonIndex;

    [MenuItem("Window/SceneSwitcher/Enable")]
    public static void Enable()
    {
        SceneView.onSceneGUIDelegate += OnSceneGUI;
        EditorSceneManager.newSceneCreated += OnSceneCreated;
        m_buttonIndex = GetCurrentSceneIndex();
    }

    private static void OnSceneCreated(Scene scene, NewSceneSetup setup, NewSceneMode mode)
    {
        m_buttonIndex = GetCurrentSceneIndex();
    }

    [MenuItem("Window/SceneSwitcher/Disable")]
    public static void Disable()
    {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
    }

    private static int GetCurrentSceneIndex()
    {
        string[] names = AssetDatabase.FindAssets("t:SceneAsset");
        int index = 0;
        foreach (string name in names)
        {
            string path = AssetDatabase.GUIDToAssetPath(names[index]);
            if (EditorSceneManager.GetActiveScene().path == path)
            {
                return index;
            }
            index++;
        }
        return 0;
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        Handles.BeginGUI();
        string[] paths = AssetDatabase.FindAssets("t:SceneAsset");
        string[] names = new string[paths.Length];
        int index = 0;
        foreach (string scene in paths)
        {
            string[] path = AssetDatabase.GUIDToAssetPath(scene).Split('/');
            string name = path[path.Length - 1].Split('.')[0];
            names[index] = name;
            index++;
        }
        GUILayout.BeginArea(new Rect(0, sceneView.position.height - 50, sceneView.position.width, 20));
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        int prevIndex = m_buttonIndex;
        m_buttonIndex = GUILayout.Toolbar(m_buttonIndex, names, null, GUI.ToolbarButtonSize.FitToContents);
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
        Handles.EndGUI();

        if (m_buttonIndex != prevIndex)
        {
            EditorSceneManager.SaveModifiedScenesIfUserWantsTo(new Scene[] { EditorSceneManager.GetActiveScene() });
            EditorSceneManager.OpenScene(AssetDatabase.GUIDToAssetPath(paths[m_buttonIndex]), OpenSceneMode.Single);
        }
    }
}
