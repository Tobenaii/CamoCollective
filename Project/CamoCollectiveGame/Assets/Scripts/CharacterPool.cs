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
        ReturnCharacterNext(player.GetCharacter());
        player.SetCharacter(GetNextCharacter());
    }

    public void GetPreviousCharacter(PlayerData player)
    {
        if (m_characterPool.Count == 0)
            return;
        ReturnCharacterPrev(player.GetCharacter());
        player.SetCharacter(GetPreviousCharacter());
    }

    private CharacterData GetNextCharacter()
    {
        if (m_characterPool.Count == 0)
            return null;
        CharacterData character = m_characterPool[0];
        m_characterPool.RemoveAt(0);
        return character;
    }

    private CharacterData GetPreviousCharacter()
    {
        if (m_characterPool.Count == 0)
            return null;
        CharacterData character = m_characterPool[m_characterPool.Count - 1];
        m_characterPool.RemoveAt(m_characterPool.Count - 1);
        return character;
    }

    private void ReturnCharacterNext(CharacterData character)
    {
        if (character == null)
            return;
        m_characterPool.Add(character);
    }

    private void ReturnCharacterPrev(CharacterData character)
    {
        if (character == null)
            return;
        m_characterPool.Insert(0, character);
    }

    public void ReturnCharacter(PlayerData player)
    {
        if (player.GetCharacter() == null)
            return;
        ReturnCharacterNext(player.GetCharacter());
        player.SetCharacter(null);
    }


}
