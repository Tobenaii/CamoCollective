using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameModeSelection : MonoBehaviour
{
    [SerializeField]
    private SceneAsset m_scene;
    [SerializeField]
    private ParticleSystem m_particles;

    public void StartScene()
    {
        SceneManager.LoadSceneAsync(m_scene.name, LoadSceneMode.Additive);
        EventSystem.current.SetSelectedGameObject(null);
    }
}
