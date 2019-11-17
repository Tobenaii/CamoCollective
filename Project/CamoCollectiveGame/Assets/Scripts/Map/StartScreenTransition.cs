using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(SceneLoader))]
[RequireComponent(typeof(RawImage))]
public class StartScreenTransition : MonoBehaviour
{
    [SerializeField]
    private float m_fadeSpeed;

    private RawImage m_image;
    private SceneLoader m_sceneLoader;
    private float m_opacity;
    private bool m_transitionOpen;
    private bool m_transitionClose;
    private bool m_isLoading = false;

    private void Start()
    {
        m_isLoading = false;
        m_sceneLoader = GetComponent<SceneLoader>();
        m_image = GetComponent<RawImage>();
        m_transitionOpen = true;
    }

    private void Update()
    {
        m_image.color = new Color(m_image.color.r, m_image.color.g, m_image.color.b, m_opacity);
        if (m_transitionOpen)
        {
            m_opacity += m_fadeSpeed * Time.deltaTime;
        }
        else if (m_transitionClose)
        {
            m_opacity -= m_fadeSpeed * Time.deltaTime;

        }
        m_opacity = Mathf.Clamp(m_opacity, 0, 1);
        if (m_opacity == 1 && !m_isLoading)
        {
            m_isLoading = true;
            m_sceneLoader.Load();
        }
        if (m_opacity == 0 && m_isLoading)
            SceneManager.UnloadSceneAsync(gameObject.scene.name);
    }

    public void EndTransition()
    {
        m_transitionOpen = false;
        m_transitionClose = true;
    }
}
