﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LifeCounter : MonoBehaviour
{
    [SerializeField]
    private Sprite m_sprite;
    [SerializeField]
    private Vector3 m_scale;
    [SerializeField]
    private Vector3 m_spacing;
    private float m_livesValue;

    private Stack<GameObject> m_uiImages = new Stack<GameObject>();

#if UNITY_EDITOR
    private bool m_destroy;
#endif

    private void Update()
    {
        if (m_livesValue == null)
            return;

#if UNITY_EDITOR
        if (EditorApplication.isPlaying && m_destroy)
        {
            foreach (GameObject obj in m_uiImages)
                DestroyImmediate(obj);
            m_uiImages.Clear();
            m_destroy = false;
        }
#endif

        while (m_uiImages.Count < m_livesValue)
        {
            GameObject image = new GameObject("LifeImage", new System.Type[] { typeof(Image) });
            image.GetComponent<Image>().sprite = m_sprite;
            image.GetComponent<Image>().SetNativeSize();
            image.transform.SetParent(transform);
            image.transform.localPosition = m_spacing * m_uiImages.Count;
            image.transform.localScale = m_scale;
            
            m_uiImages.Push(image);
        }

        while (m_uiImages.Count > m_livesValue && m_livesValue >= 0)
        {
            GameObject obj = m_uiImages.Pop();
            Destroy(obj);
        }
    }

    public void SetLivesValue(float value)
    {
        m_livesValue = value;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (EditorApplication.isPlaying)
            m_destroy = true;
    }
#endif
}
