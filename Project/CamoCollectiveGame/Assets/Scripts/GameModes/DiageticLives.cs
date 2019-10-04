using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiageticLives : MonoBehaviour
{
    [SerializeField]
    private GameObject m_sprite;
    [SerializeField]
    private FloatReference m_lives;
    [SerializeField]
    private List<GameObject> m_livesObjects = new List<GameObject>();
    [SerializeField]
    private float m_distanceFromCentre;
    [SerializeField]
    private float m_angleOffset;

    float m_currentAngleOffset;

    // Start is called before the first frame update
    void Start()
    {
        UpdatePositions();
    }

    public void UpdatePositions()
    {
        float angle = m_angleOffset;
        bool left = true;

        if (m_lives.Value % 2 != 0)
            UpdateGameObject(0, 0, true);

        for (int i = (m_lives.Value % 2 != 0)?1:0; i < m_lives.Value; i++)
        {
            UpdateGameObject(i, angle, left);
            left = !left;
            if (left)
                angle += m_angleOffset;
        }
        for (int i = (int)m_lives.Value; i < m_livesObjects.Count; i++)
        {
            m_livesObjects[i].SetActive(false);
        }
    }

    private void UpdateGameObject(int index, float angle, bool left)
    {
        if (index >= m_livesObjects.Count)
            m_livesObjects.Add(Instantiate(m_sprite, Vector3.zero, Quaternion.identity, transform));
        GameObject sprite = m_livesObjects[index];
        sprite.transform.localPosition = Vector3.zero;
        sprite.transform.localRotation = Quaternion.identity;
        m_currentAngleOffset = 90 + (Vector3.Angle(Vector3.right, transform.parent.forward) * ((transform.parent.forward.z > 0) ? 1 : -1));
        sprite.transform.RotateAround(transform.position, Vector3.up, angle * (left ? -1 : 1) + m_currentAngleOffset);
        sprite.transform.position += sprite.transform.up * m_distanceFromCentre;
        sprite.gameObject.SetActive(true);
        angle += m_angleOffset * (left ? 1 : 0);
        left = !left;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePositions();
    }

    private void OnValidate()
    {
    }
}
