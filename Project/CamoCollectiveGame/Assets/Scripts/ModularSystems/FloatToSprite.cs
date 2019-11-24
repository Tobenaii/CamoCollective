using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatToSprite : MonoBehaviour
{
    [SerializeField]
    private FloatReference m_floatValue;
    [SerializeField]
    private int m_offset;
    [SerializeField]
    private Image m_image;
    [SerializeField]
    private List<Sprite> m_sprites;

    private void Update()
    {
        m_image.sprite = m_sprites[(int)Mathf.Floor(m_floatValue.Value) + m_offset - 1];
        m_image.SetNativeSize();
    }
}
