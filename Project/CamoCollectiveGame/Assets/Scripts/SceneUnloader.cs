using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUnloader : MonoBehaviour
{
    [SerializeField]
    private List<SceneAsset> m_scenes;
    [SerializeField]
    private bool m_unloadOnAwake = true;

    private void Awake()
    {
        if (m_unloadOnAwake)
            UnloadScene();
    }

    public void UnloadScene()
    {
        foreach (SceneAsset scene in m_scenes)
        {
            if (!SceneManager.GetSceneByName(scene.name).IsValid())
                return;
            SceneManager.UnloadSceneAsync(scene.name);
        }
    }
}
