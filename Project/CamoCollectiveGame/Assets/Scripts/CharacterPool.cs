using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPool : MonoBehaviour
{
    [SerializeField]
    private List<CharacterData> m_characterPool;

    private void Awake()
    {
    }

    public void GetNextCharacter(PlayerData player)
    {
        if (m_characterPool.Count == 0)
            return;
        ReturnCharacter(player.GetCharacter());
        player.SetCharacter(GetNextCharacter());
    }

    public CharacterData GetNextCharacter()
    {
        if (m_characterPool.Count == 0)
            return null;
        CharacterData character = m_characterPool[0];
        m_characterPool.RemoveAt(0);
        return character;
    }

    public void ReturnCharacter(CharacterData character)
    {
        if (character == null)
            return;
        m_characterPool.Add(character);
    }

    public void ReturnCharacter(PlayerData player)
    {
        if (player.GetCharacter() == null)
            return;
        ReturnCharacter(player.GetCharacter());
        player.SetCharacter(null);
    }


}
