﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameObjectPoolInstance))]
public class HairController : MonoBehaviour
{
    [SerializeField]
    private FloatValue m_moveSpeed;
    [SerializeField]
    private BoolValue m_moveValue;
    private Renderer m_renderer;
    private bool m_moveHair;
    private float m_offset;
    private GameObjectPoolInstance m_poolInst;

    private void Awake()
    {
        m_renderer = GetComponent<Renderer>();
        m_moveHair = false;
        m_poolInst = GetComponent<GameObjectPoolInstance>();
    }

    private void Update()
    {
        if (!m_moveValue.Value)
            return;
        transform.position += Vector3.down * m_moveSpeed.Value * Time.deltaTime;

        if (transform.position.y < -5)
            m_poolInst.Pool.DestroyObject(gameObject);
    }
}
