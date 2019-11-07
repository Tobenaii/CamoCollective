﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoultryBashNPC : MonoBehaviour
{
    private Animator m_animator;
    private Material m_material;

    private void Awake()
    {
        m_animator = GetComponentInChildren<Animator>();
        m_animator.speed = Random.Range(1.0f, 1.5f);
    }

    public void Cheer()
    {
        m_animator.SetTrigger("Cheer");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
