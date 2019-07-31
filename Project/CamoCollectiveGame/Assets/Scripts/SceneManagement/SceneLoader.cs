using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private bool m_runOnAwake;
    [SerializeField]
    private List<SceneValue> m_scenesToUnload;
    [SerializeField]
    private List<SceneValue> m_scenesToLoad;

    private int m_loadedAmmount;
    private int m_loadSceneAmmount;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (m_runOnAwake)
            Load();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void Load()
    {
        m_loadSceneAmmount = 0;
        m_loadedAmmount = 0;
        LoadScenes();
        if (m_loadSceneAmmount == 0)
            UnloadScenes();
    }

    private void LoadScenes()
    {
        foreach (SceneValue value in m_scenesToLoad)
        {
            if (!SceneManager.GetSceneByName(value).IsValid())
            {
                SceneManager.LoadSceneAsync(value, LoadSceneMode.Additive);
                m_loadSceneAmmount++;
            }
        }
    }

    private void UnloadScenes()
    {
        foreach (SceneValue scene in m_scenesToUnload)
        {
            if (SceneManager.GetSceneByName(scene).IsValid())
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        m_loadedAmmount++;
        if (m_loadedAmmount == m_loadSceneAmmount)
            UnloadScenes();
    }
}
