using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryModeCharacterController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_gainedPointText;
    private Vector3 m_target;

    private void Start()
    {
        m_target = transform.position + transform.forward * 5;
    }

    private void Update()
    {
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
