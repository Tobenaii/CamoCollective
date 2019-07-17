using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeCounter : MonoBehaviour
{
    [SerializeField]
    private Sprite m_sprite;
    [SerializeField]
    private Vector3 m_scale;
    [SerializeField]
    private Vector3 m_spacing;
    [SerializeField]
    private FloatReference m_livesValue;

    private Stack<GameObject> m_uiImages = new Stack<GameObject>();

    private void Update()
    {
        while (m_uiImages.Count < m_livesValue.Value)
        {
            GameObject image = new GameObject("LifeImage", new System.Type[] { typeof(Image) });
            image.GetComponent<Image>().sprite = m_sprite;
            image.transform.SetParent(transform);
            image.transform.localPosition = m_spacing * m_uiImages.Count;
            image.transform.localScale = m_scale;
            m_uiImages.Push(image);
        }

        while (m_uiImages.Count > m_livesValue.Value && m_livesValue.Value >= 0)
        {
            GameObject obj = m_uiImages.Pop();
            Destroy(obj);
        }
    }

}
