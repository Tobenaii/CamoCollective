using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChonkJousterRespawner : MonoBehaviour
{
    [SerializeField]
    private Transform m_respawnPoint;
    [SerializeField]
    private float m_respawnWaitForOtherTime;
    [SerializeField]
    private Animator m_animator;
    private List<GameObject> m_chonks = new List<GameObject>();
    private bool m_respawning;
    int spawned = 0;

    public void Respawn(GameObject chonk)
    {
        chonk.gameObject.SetActive(false);
        chonk.transform.position = m_respawnPoint.position;
        chonk.transform.rotation = m_respawnPoint.rotation;
        m_chonks.Add(chonk);
        Startup();
    }

    public void Respawn()
    {
        StartCoroutine(DoQueuedRespawn());
    }

    private void Respawned()
    {
        m_respawning = false;
        if (m_chonks.Count != 0)
            Respawn(); 
    }

    public void OnSpawned()
    {
        if (m_chonks.Count == 0 && spawned == 0)
            Cleanup();
        else
            spawned--;
        if (spawned < 0)
            spawned = 0;
    }

    IEnumerator DoQueuedRespawn()
    {
        while (m_respawning)
            yield return null;
        m_respawning = true;
        if (m_chonks.Count != 0)
        {
            spawned++;
            GameObject chonk = m_chonks[0];
            chonk.SetActive(true);
            chonk.GetComponent<ChonkJoustingController>().Respawn(m_respawnPoint);
            m_chonks.RemoveAt(0);
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
        m_animator.SetTrigger("Open");
    }

    private void Cleanup()
    {
        m_animator.SetTrigger("Close");
    }


}
