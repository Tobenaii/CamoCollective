using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorTrigger : MonoBehaviour
{
    [SerializeField]
    private Animator m_animator;

    private void Awake()
    {
        m_animator.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        m_animator.enabled = true;
    }
}
