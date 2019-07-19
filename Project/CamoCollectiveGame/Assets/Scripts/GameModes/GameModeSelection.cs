using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SceneValue
{
    public static implicit operator string(SceneValue value)
    {
        return value.m_sceneName;
    }
#if UNITY_EDITOR
    [SerializeField]
    public SceneAsset m_scene;
#endif
    [SerializeField]
    private string m_sceneName;
}

public class GameModeSelection : MonoBehaviour
{
    [SerializeField]
    private SceneValue m_scene;
    [SerializeField]
    private ParticleSystem m_particles;

    public void StartScene()
    {
        SceneManager.LoadSceneAsync(m_scene, LoadSceneMode.Additive);
        EventSystem.current.SetSelectedGameObject(null);
    }
}
