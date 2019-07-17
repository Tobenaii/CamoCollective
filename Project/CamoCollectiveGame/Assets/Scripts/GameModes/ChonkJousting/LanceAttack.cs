using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanceAttack : MonoBehaviour
{
    [SerializeField]
    private float m_knockbackForce;
    [SerializeField]
    private FloatValue m_livesValue;
    [SerializeField]
    private FloatValue m_scoreValue;
    [SerializeField]
    private float m_shieldAngle;

    private void Awake()
    {
        m_livesValue.value = 4;
        m_scoreValue.value = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        LanceAttack chonk = other.transform.parent.GetComponent<LanceAttack>();
        if (chonk == null)
            return;
        chonk.Attack(gameObject, m_scoreValue);
    }

    public void Attack(GameObject other, FloatValue otherScore)
    {
        float dot = Vector3.Dot(transform.forward, other.transform.forward);
        float shield = Mathf.Lerp(-1, 1, Mathf.InverseLerp(0, 180, m_shieldAngle));
        if (dot < shield)
            return;
        GetComponent<Rigidbody>().AddForce(other.transform.forward * m_knockbackForce, ForceMode.Impulse);
        m_livesValue.value--;
        otherScore.value++;
    }
}
