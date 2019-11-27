using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorTrigger : MonoBehaviour
{
    [SerializeField]
    private Animator m_animator;
    [SerializeField]
    private string m_trigger;
    [SerializeField]
    private string m_disableTrigger;

    private void OnTriggerEnter(Collider other)
    {
        m_animator.ResetTrigger(m_disableTrigger);
        m_animator.SetTrigger(m_trigger);
    }
    public void DisableAnimator()
    {
        m_animator.SetTrigger(m_disableTrigger);
    }
}
