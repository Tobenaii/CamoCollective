using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUnloader : MonoBehaviour
{
    [SerializeField]
    private SceneAsset m_scene;

    private void Awake()
    {
        if (!SceneManager.GetSceneByName(m_scene.name).IsValid())
            return;
        SceneManager.UnloadSceneAsync(m_scene.name);
    }
}
