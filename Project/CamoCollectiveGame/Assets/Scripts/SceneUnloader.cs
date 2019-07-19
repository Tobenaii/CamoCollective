using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUnloader : MonoBehaviour
{
    [SerializeField]
    private List<SceneValue> m_scenes;
    [SerializeField]
    private bool m_unloadOnAwake = true;

    private void Awake()
    {
        if (m_unloadOnAwake)
            UnloadScene();
    }

    public void UnloadScene()
    {
        foreach (SceneValue scene in m_scenes)
        {
            if (!SceneManager.GetSceneByName(scene).IsValid())
                return;
            SceneManager.UnloadSceneAsync(scene);
        }
    }
}
