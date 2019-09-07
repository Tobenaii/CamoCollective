using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterIconSetter : MonoBehaviour
{
    [SerializeField]
    private PlayerData m_player;

    [SerializeField]
    private BoolReference m_isDeadValue;
    [SerializeField]
    private Color m_deadColourHue;

    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        image.sprite = m_player.Character.Icon;
        image.color = Color.white;
    }

    private void Update()
    {
        if (image.sprite == null)
            image.sprite = m_player.Character.Icon;
        if (m_isDeadValue != null && m_isDeadValue.Value)
            image.color = m_deadColourHue;
    }

}
