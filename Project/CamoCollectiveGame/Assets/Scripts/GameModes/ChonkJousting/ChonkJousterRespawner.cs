using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChonkJousterRespawner : MonoBehaviour
{
    [SerializeField]
    private Transform m_respawnPoint;
    [SerializeField]
    private Animator m_animator;
    private List<GameObject> m_chonks = new List<GameObject>();
    private bool m_respawning;
    private int m_chonkIndex = 0;

    public void QueueRespawn(GameObject chonk)
    {
        chonk.gameObject.SetActive(false);
        m_chonks.Add(chonk);
        Startup();
    }

    public void Respawn()
    {
        if (m_chonks.Count == 0)
            return;
        StartCoroutine(DoQueuedRespawn(m_chonks[m_chonkIndex]));
        m_respawning = true;
        m_chonkIndex++;
    }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  

    public void OnSpawned()
    {
        m_respawning = false;
        m_chonks.RemoveAt(0);
        m_chonkIndex--;
        if (m_chonks.Count == 0)
            Cleanup();
        else
            Respawn();
    }

    IEnumerator DoQueuedRespawn(GameObject chonk)
    {
        while (m_respawning)
            yield return null;

        chonk.transform.position = m_respawnPoint.position;
        chonk.transform.rotation = m_respawnPoint.rotation;
        chonk.SetActive(true);
        chonk.GetComponent<ChonkJoustingController>().Respawn();
    }

    private void Startup()
    {
        if (m_chonks.Count > 1)
            return;
        m_animator.SetTrigger("Open");
    }

    private void Cleanup()
    {
        if (m_chonks.Count > 0)
            return;
        m_animator.SetTrigger("Close");
    }


}
