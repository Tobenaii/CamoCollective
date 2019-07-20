using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChonkJoustingData))]
public class ChonkJoustingLanceAttack : MonoBehaviour
{
    [SerializeField]
    private float m_knockbackForce;
    [SerializeField]
    private float m_knockupForce;
    [SerializeField]
    private float m_shieldAngle;

    private void OnTriggerEnter(Collider other)
    {
        ChonkJoustingLanceAttack chonk = other.transform.parent?.GetComponent<ChonkJoustingLanceAttack>();
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
        GetComponent<ChonkJoustingData>().AddScore(1);
        other.GetComponent<ChonkJoustingLanceAttack>().Hurt(transform.forward * m_knockbackForce, m_knockupForce);
    }

    public void Hurt(Vector3 knockback, float knockup)
    {
        GetComponent<Rigidbody>().AddForce(Vector3.up * knockup, ForceMode.Impulse);
        GetComponent<Rigidbody>().AddForce(knockback, ForceMode.Impulse);
        GetComponent<ChonkJoustingData>().RemoveLives(1);
        if (GetComponent<ChonkJoustingData>().GetLives() <= 0)
            GetComponent<ChonkJoustingDeath>().OnDeath();
    }
}
