using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChonkJousterRespawner : MonoBehaviour
{
    [SerializeField]
    private Transform m_respawnPoint;
    [SerializeField]
    private Animator m_animator;
    [SerializeField]
    private float m_respawnForce;
    private List<GameObject> m_chonks = new List<GameObject>();
    private int m_chonkIndex = 0;
    private bool m_isOpen;
    private bool m_isRespawning;

    public void QueueRespawn(GameObject chonk)
    {
        chonk.gameObject.SetActive(false);
        m_chonks.Add(chonk);
        Startup();
    }

    public void Respawn()
    {
        if (m_chonks.Count == 0 || m_chonkIndex == m_chonks.Count)
            return;
        if (!m_isOpen)
        {
            Startup();
            return;
        }
        StartCoroutine(DoQueuedRespawn(m_chonks[m_chonkIndex]));
        m_chonkIndex++;
    }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  

    public void OnSpawned()
    {
        m_isRespawning = false;
        m_chonks.RemoveAt(0);
        m_chonkIndex--;
        if (m_chonks.Count == 0)
            Cleanup();
        else
            Respawn();
    }

    IEnumerator DoQueuedRespawn(GameObject chonk)
    {
        while (m_isRespawning)
            yield return null;
        m_isRespawning = true;
        chonk.transform.position = m_respawnPoint.position;
        chonk.transform.rotation = m_respawnPoint.rotation;
        chonk.SetActive(true);
        Rigidbody rb = chonk.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        //rb.useGravity = false;
        rb.detectCollisions = true;

        StandardCharacterController controller = chonk.GetComponent<StandardCharacterController>();
        ChonkJouster jouster = chonk.GetComponent<ChonkJouster>();
        //jouster.Reinit();
        controller.enabled = true;
        float timer = 1;
        while (timer > 0)
        {
            controller.OverrideVelocity(chonk.transform.forward * 10);
            timer -= Time.deltaTime;
            yield return null;
        }
        //rb.AddForce(chonk.transform.forward * m_respawnForce, ForceMode.Impulse);
    }

    private void Startup()
    {
        if (m_chonks.Count > 1)
            return;
        if (m_isOpen)
        {
            Respawn();
            return;
        }
        m_animator.SetTrigger("Open");
        m_animator.ResetTrigger("Close");
        m_isOpen = true;
    }

    private void Cleanup()
    {
        if (m_chonks.Count > 0)
            return;
        m_animator.SetTrigger("Close");
        m_animator.ResetTrigger("Open");
        m_isOpen = false;
    }


}
