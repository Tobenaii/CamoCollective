using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/PlayerDataReference")]
public class PlayerDataReference : ScriptableObject
{
    [System.NonSerialized]
    public PlayerData value;
}
