using System;
using UnityEngine;

[Serializable]
public class Tile : MonoBehaviour
{
    public int X;

    public int Y;

    public DiscoveryState DiscoveryState;

    public bool Walkable;

    public override string ToString() => $"({X} | {Y})";
}

public enum DiscoveryState
{
    Hidden,
    Discovered,
    Visible
}
