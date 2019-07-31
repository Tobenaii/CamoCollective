﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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