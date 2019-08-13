using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterIconSetter : MonoBehaviour
{
    [SerializeField]
    private PlayerData m_player;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().sprite = m_player.Character.Icon;
    }

}
