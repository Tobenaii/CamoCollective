using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PlayerDataUnityEvent : UnityEvent<PlayerData>
{

}

public class ButtonEvent : MonoBehaviour
{
    [SerializeField]
    private PlayerDataUnityEvent m_onCursorEnter;
    [SerializeField]
    private PlayerDataUnityEvent m_onCursorExit;
    [SerializeField]
    private PlayerDataUnityEvent m_onCursorClick;

    public PlayerDataUnityEvent OnCursorEnter => m_onCursorEnter;
    public PlayerDataUnityEvent OnCursorExit => m_onCursorExit;
    public PlayerDataUnityEvent OnCursorClick => m_onCursorClick;
}
