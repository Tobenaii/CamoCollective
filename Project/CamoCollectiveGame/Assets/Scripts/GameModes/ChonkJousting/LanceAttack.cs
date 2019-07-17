using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanceAttack : MonoBehaviour
{
    [SerializeField]
    private float m_knockbackForce;
    [SerializeField]
    private float m_knockupForce;
    [SerializeField]
    private FloatValue m_livesValue;
    [SerializeField]
    private FloatValue m_scoreValue;
    [SerializeField]
    private float m_shieldAngle;

    private void OnTriggerEnter(Collider other)
    {
        LanceAttack chonk = other.transform.parent?.GetComponent<LanceAttack>();
        if (chonk == null)
            return;
        Attack(chonk.gameObject);
    }

    public void Attack(GameObject other)
    {
        float dot = Vector3.Dot(transform.forward, other.transform.forward);
        float shield = Mathf.Lerp(-1, 1, Mathf.InverseLerp(0, 180, m_shieldAngle));
        if (dot < shield)
            return;
        m_scoreValue.value++;
        other.GetComponent<LanceAttack>().Hurt(transform.forward * m_knockbackForce, m_knockupForce);
    }

    public void Hurt(Vector3 knockback, float knockup)
    {
        GetComponent<Rigidbody>().AddForce(Vector3.up * knockup, ForceMode.Impulse);
        GetComponent<Rigidbody>().AddForce(knockback, ForceMode.Impulse);
        m_livesValue.value--;
        if (m_livesValue.value <= 0)
            GetComponent<ChonkJoustingDeath>().OnDeath();
    }
}
