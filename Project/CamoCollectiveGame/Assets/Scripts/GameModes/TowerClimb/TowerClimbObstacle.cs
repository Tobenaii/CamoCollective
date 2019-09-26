using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameObjectPoolInstance))]
public class TowerClimbObstacle : MonoBehaviour
{
    [SerializeField]
    private FloatValue m_fallSpeed;
    [SerializeField]
    private BoolValue m_moveValue;
    private GameObjectPoolInstance m_poolInst;

    private void Awake()
    {
        m_poolInst = GetComponent<GameObjectPoolInstance>();
    }

    public void Destruct()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!m_moveValue.Value)
            return;
        transform.position += Vector3.down * m_fallSpeed.Value * Time.deltaTime;

        if (transform.position.y < -5)
            m_poolInst.Pool.DestroyObject(gameObject);
    }
}
