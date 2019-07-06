using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "SceneSystem/SceneGrid")]
public class SceneGrid : ScriptableObject
{
    [SerializeField]
    private List<SceneAsset> m_sceneGrid = new List<SceneAsset>();

    [System.Obsolete]
    public void LoadScenes()
    {
        for (int i = 0; i < EditorSceneManager.sceneCount; i++)
            EditorSceneManager.UnloadScene(EditorSceneManager.GetSceneAt(i));

        foreach (SceneAsset scene in m_sceneGrid)
            EditorSceneManager.OpenScene("Assets/Scenes/" + scene.name + ".unity", OpenSceneMode.Additive);
    }
}
