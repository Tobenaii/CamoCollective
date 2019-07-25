using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/TowerClimber")]
public class TowerClimber : ScriptableObject, IEquatable<TowerClimber>, IComparable<TowerClimber>
{
    [HideInInspector]
    public int placement;
    [HideInInspector]
    public PlayerData player;
    [HideInInspector]
    public GameObject towerClimber;
    [HideInInspector]
    public bool isDead;

    public int CompareTo(TowerClimber other)
    {
        if (other == null)
            return 1;
        return towerClimber.transform.position.y.CompareTo(other.towerClimber.transform.position.y);
    }

    public bool Equals(TowerClimber other)
    {
        if (other == null)
            return false;
        return (towerClimber.transform.position.y.Equals(other.towerClimber.transform.position.y));
    }
    public override int GetHashCode()
    {
        return placement.GetHashCode();
    }
}
