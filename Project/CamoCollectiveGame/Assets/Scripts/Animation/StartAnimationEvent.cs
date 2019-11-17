using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimationEvent : MonoBehaviour
{
    [SerializeField]
    private Animator m_animator;

    public void SetTrigger(string trigger)
    {
        m_animator.SetTrigger(trigger);
    }
}
