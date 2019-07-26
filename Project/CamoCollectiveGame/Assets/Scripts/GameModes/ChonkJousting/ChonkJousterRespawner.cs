using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChonkJousterRespawner : MonoBehaviour
{
    [SerializeField]
    private Transform m_respawnPoint;
    [SerializeField]
    private float m_respawnWaitForOtherTime;
    private Animator m_animator;
    private List<GameObject> m_chonks = new List<GameObject>();
    private bool m_respawning;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    public void Respawn(GameObject chonk)
    {
        chonk.gameObject.SetActive(false);
        chonk.transform.position = m_respawnPoint.position;
        chonk.transform.rotation = m_respawnPoint.rotation;
        m_chonks.Add(chonk);
        Respawn();
    }

    public void Respawn()
    {
        StartCoroutine(DoQueuedRespawn());
    }

    private void Respawned()
    {
        m_respawning = false;
        m_chonks.RemoveAt(0);
        if (m_chonks.Count != 0)
            Respawn();
        else
            Cleanup();
    }

    IEnumerator DoQueuedRespawn()
    {
        while (m_respawning)
            yield return null;
        m_respawning = true;
        if (m_chonks.Count != 0)
        {
            GameObject chonk = m_chonks[0];
            chonk.SetActive(true);
            chonk.GetComponent<ChonkJoustingController>().Respawn(m_respawnPoint);

            float timer = m_respawnWaitForOtherTime;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }
            Respawned();
        }
    }

    private void Startup()
    {
        //m_animator.SetTrigger("Open");
    }

    private void Cleanup()
    {
        //m_animator.SetTrigger("Close");
    }


}
