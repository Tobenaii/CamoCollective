using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryModeCharacterController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_gainedPointText;
    [SerializeField]
    private GameObjectListSet m_targetListSet;
    private Animator m_animator;
    private Vector3 m_firstTarget;
    private Vector3 m_target;
    private bool m_doTheShuffle;
    private Vector3 m_velocity;
    private bool m_stopped;

    private void Start()
    {
        m_target = m_targetListSet[0].transform.position;
        m_targetListSet.RemoveAt(0);
        m_firstTarget = new Vector3(transform.position.x, transform.position.y, m_target.z - (m_target.z - transform.position.z) / 2);

        m_animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (m_stopped)
        {
            transform.forward = Vector3.MoveTowards(transform.forward, Vector3.back, 10 * Time.deltaTime);
            return;
        }
        transform.forward = m_velocity;
        if (!m_doTheShuffle)
        {
            transform.position = Vector3.SmoothDamp(transform.position, m_firstTarget - Vector3.forward * 10, ref m_velocity, 4.0f);
            if (Vector3.Distance(transform.position, m_firstTarget) < 0.1f)
                m_doTheShuffle = true;
        }
        else if (Vector3.Distance(transform.position, m_target) > 0.5f)
            transform.position = Vector3.SmoothDamp(transform.position, m_target, ref m_velocity, 1.0f);
        else
        {
            m_stopped = true;
            m_animator.SetTrigger("Idle");
            m_animator.speed = 1;
        }
    }

    public void GainPoint()
    {
        m_gainedPointText.SetActive(true);
        StartCoroutine(What());
    }

    IEnumerator What()
    {
        yield return new WaitForSeconds(1);
        m_gainedPointText.SetActive(false);
    }
}
