using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadOnTrigger : MonoBehaviour
{
    [SerializeField]
    private string m_sceneName;
    public void LoadScene()
    {
        SceneManager.LoadSceneAsync(m_sceneName, LoadSceneMode.Additive);
    }
}
