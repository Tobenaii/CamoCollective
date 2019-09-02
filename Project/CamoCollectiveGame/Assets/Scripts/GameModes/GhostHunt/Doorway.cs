using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doorway : MonoBehaviour
{
    [SerializeField]
    private GameObjectListSet m_doorwayListSet;
    [SerializeField]
    private float m_timeToExit;

    private void Start()
    {
        m_doorwayListSet.Add(gameObject);
    }

    public void ExitAnotherDoorway(GameObject obj)
    {
        StartCoroutine(ExitDoorway(obj));
    }

    private IEnumerator ExitDoorway(GameObject obj)
    {
        obj.SetActive(false);
        float timer = m_timeToExit;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        m_doorwayListSet.Remove(gameObject);
        int index = Random.Range(0, m_doorwayListSet.Count);
        GameObject doorway = m_doorwayListSet[index];
        obj.transform.position = doorway.transform.position;
        obj.transform.rotation = doorway.transform.rotation;
        obj.SetActive(true);
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.AddForce(doorway.transform.forward * 20, ForceMode.Impulse);
        m_doorwayListSet.Add(gameObject);
    }

}
