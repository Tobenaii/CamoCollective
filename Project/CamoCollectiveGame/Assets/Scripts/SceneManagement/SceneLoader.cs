using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private BoolValue m_isInputDisabled;
    [SerializeField]
    private bool m_runOnAwake;
    [SerializeField]
    private bool m_unloadAllScenes;
    [SerializeField]
    private List<SceneValue> m_scenesToUnload;
    [SerializeField]
    private List<SceneValue> m_scenesToLoad;
    [SerializeField]
    private BoolReference m_override;
    [SerializeField]
    private List<SceneValue> m_scenesToLoadOverride;
    [SerializeField]
    private GameEvent m_loadedEvent;

    private int m_unloadAmmount;
    private int m_unloadedAmmount;

    private int m_loadAmmount;
    private int m_loadedAmmount;

    private void Start()
    {
        if (m_runOnAwake)
            Load();
    }

    private void OnDestroy()
    {
    }

    public void Load()
    {
        m_isInputDisabled.Value = true;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        m_unloadedAmmount = 0;
        if (m_unloadAllScenes)
            UnloadAllScenes();
        else
            UnloadScenes();
    }

    private void LoadScenes(List<SceneValue> scenes)
    {
        if (scenes.Count == 0)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            m_loadedEvent?.Invoke();
            return;
        }
        m_loadAmmount = scenes.Count;
        foreach (SceneValue scene in scenes)
        {
            if (!m_unloadAllScenes && SceneManager.GetSceneByName(scene).IsValid())
            {
                m_loadedAmmount++;
                CheckLoaded();
                continue;
            }
            SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        }
    }

    public void CheckLoaded()
    {
        if (m_loadedAmmount == m_loadAmmount)
        {
            m_isInputDisabled.Value = false;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            m_loadedEvent?.Invoke();
        }
    }

    private void UnloadScenes()
    {
        if (m_scenesToUnload.Count == 0)
        {
            LoadScenes((m_override.Value)?m_scenesToLoadOverride:m_scenesToLoad);
            return;
        }
        m_unloadAmmount = m_scenesToUnload.Count;
        foreach (SceneValue scene in m_scenesToUnload)
        {
            SceneManager.UnloadSceneAsync(scene);
        }
    }

    private void UnloadAllScenes()
    {
        m_unloadAmmount = SceneManager.sceneCount - 1;
        if (m_unloadAmmount == 0)
        {
            LoadScenes((m_override.Value) ? m_scenesToLoadOverride : m_scenesToLoad);
            return;
        }
        for (int i = 0; i < m_unloadAmmount + 1; i++)
        {
            if (SceneManager.GetSceneAt(i).name != gameObject.scene.name)
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        m_loadedAmmount++;
        CheckLoaded();
    }

    private void OnSceneUnloaded(Scene scene)
    {
        m_unloadedAmmount++;
        if (m_unloadedAmmount == m_unloadAmmount)
            LoadScenes((m_override.Value) ? m_scenesToLoadOverride : m_scenesToLoad);
    }
}
