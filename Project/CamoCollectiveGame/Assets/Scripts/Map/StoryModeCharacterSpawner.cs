using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryModeCharacterSpawner : MonoBehaviour
{
    [SerializeField]
    private List<PlayerData> m_players;
    [SerializeField]
    private Vector3 m_spacing;


    // Start is called before the first frame update
    void Start()
    {
        int index = 0;
        foreach (PlayerData player in m_players)
        {
            if (player.IsPlaying)
            {
                GameObject character = Instantiate(player.Character.StoryModeCharacter, transform.position + m_spacing * index, transform.rotation, transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
