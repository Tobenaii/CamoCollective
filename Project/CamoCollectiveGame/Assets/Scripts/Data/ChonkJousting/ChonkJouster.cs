using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/ChonkJouster")]
public class ChonkJouster : ScriptableObject
{
    [HideInInspector]
    public int score;
    [HideInInspector]
    public int lives;
    [HideInInspector]
    public PlayerData player;
    [HideInInspector]
    public float respawnTimer;
    [HideInInspector]
    public bool isDead;
}
