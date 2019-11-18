using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryModeCharacterController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_gainedPointText;
    [SerializeField]
    private GameObjectListSet m_targetListSet;
    private Vector3 m_firstTarget;
    private Vector3 m_target;
    private bool m_doTheShuffle;

    private void Start()
    {
        m_target = m_targetListSet[0].transform.position;
        m_targetListSet.RemoveAt(0);
        m_firstTarget = new Vector3(transform.position.x, transform.position.y, m_target.z);
    }

    private void Update()
    {
        if (!m_doTheShuffle)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_firstTarget, 2 * Time.deltaTime);
            if (Vector3.Distance(transform.position, m_firstTarget) < 0.01f)
                m_doTheShuffle = true;
        }
        else
            transform.position = Vector3.MoveTowards(transform.position, m_target, 2 * Time.deltaTime);
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
