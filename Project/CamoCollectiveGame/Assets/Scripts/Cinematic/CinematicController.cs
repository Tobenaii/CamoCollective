using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StringEventListener))]
public class CinematicController : MonoBehaviour
{
    [SerializeField]
    private Animator m_animator;
    [SerializeField]
    private string startupCinematic;

    private void Start()
    {
        StartCinematic(startupCinematic);
    }

    public void StartCinematic(string cinematic)
    {
        if (cinematic == "StopCinematics")
        {
            m_animator.enabled = false;
            return;
        }
        m_animator.enabled = true;
        m_animator.Play(cinematic, 0);
    }
}
