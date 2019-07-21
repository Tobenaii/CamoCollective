using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader: MonoBehaviour
{
    [SerializeField]
    private List<SceneValue> m_scenes;
    public void LoadScene()
    {
        foreach (SceneValue scene in m_scenes)
        {
            if (!SceneManager.GetSceneByName(scene).IsValid())
                SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        }
    }
}
