using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUnloader : MonoBehaviour
{
    [SerializeField]
    private SceneAsset m_scene;
    [SerializeField]
    private bool m_retryUntilSuccessful;

    private void Awake()
    {
        if (!SceneManager.GetSceneByName(m_scene.name).IsValid() && !m_retryUntilSuccessful)
            return;
        if (!m_retryUntilSuccessful)
            SceneManager.UnloadSceneAsync(m_scene.name);
        else
            StartCoroutine(RetryUnload());
    }

    private IEnumerator RetryUnload()
    {
        while (!SceneManager.GetSceneByName(m_scene.name).IsValid())
        {
            yield return null;
        }
        SceneManager.UnloadSceneAsync(m_scene.name);
    }
}
