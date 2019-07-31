using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (cinematic == "StopCinematics" || cinematic == "")
        {
            m_animator.enabled = false;
            return;
        }
        m_animator.enabled = true;
        m_animator.Play(cinematic, 0);
    }
}
