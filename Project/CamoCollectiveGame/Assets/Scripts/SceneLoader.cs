using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader: MonoBehaviour
{
    [SerializeField]
    private SceneValue m_scene;
    public void LoadScene()
    {
        SceneManager.LoadSceneAsync(m_scene, LoadSceneMode.Additive);
    }
}
