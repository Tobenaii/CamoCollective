using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PoultryBashEndRound : MonoBehaviour
{
    [SerializeField]
    private Animator m_animator;

    private bool m_roundEnded;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
        SceneManager.sceneUnloaded += OnSceneUnload;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
        SceneManager.sceneUnloaded -= OnSceneUnload;
    }

    public void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "PoultryBashScene")
            return;
        if (m_roundEnded)
            StartRound();
    }

    public void OnSceneUnload(Scene scene)
    {
        if (scene.name != "PoultryBashScene")
            return;
        EndRound();
    }

    public void LoadScenes()
    {
        SceneManager.UnloadSceneAsync("PoultryBashScene");
        SceneManager.LoadSceneAsync("PoultryBashScene", LoadSceneMode.Additive);
    }

    public void EndRound()
    {
        m_animator.ResetTrigger("Close");
        m_animator.SetTrigger("Open");
        m_roundEnded = true;
    }

    private void StartRound()
    {
        m_animator.ResetTrigger("Open");
        m_animator.SetTrigger("Close");
    }
}
