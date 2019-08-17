using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectOpener : MonoBehaviour
{
    [SerializeField]
    private BoolValue m_isOpen;
    [SerializeField]
    private List<PlayerData> m_playerData;
    [SerializeField]
    private GameEvent m_closeEvent;
    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_animator.enabled = m_isOpen.Value;
    }

    private void Start()
    {
        if (m_isOpen.Value || !CheckForPlayers())
            OpenCharacterSelect();
        else
            CloseCharacterSelect();
    }

    private bool CheckForPlayers()
    {
        bool hasPlayer = false;
        foreach (PlayerData player in m_playerData)
        {
            if (player.IsPlaying)
            {
                hasPlayer = true;
                break;
            }
        }
        return hasPlayer;
    }

    public void OpenCharacterSelect()
    {
        m_animator.enabled = true;
        m_isOpen.Value = true;
        m_animator.SetTrigger("Open");
        m_animator.ResetTrigger("Close");
    }
    
    public void CloseCharacterSelect()
    {
        m_closeEvent.Invoke();
        m_isOpen.Value = false;
        m_animator.SetTrigger("Close");
        m_animator.ResetTrigger("Open");
    }
}
