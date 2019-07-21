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

    public int CompareTo(TowerClimber other)
    {
        if (other == null)
            return 1;
        return placement.CompareTo(other.placement);
    }

    public bool Equals(TowerClimber other)
    {
        if (other == null)
            return false;
        return (placement.Equals(other.placement));
    }
    public override int GetHashCode()
    {
        return placement.GetHashCode();
    }
}
