using System;
using UnityEngine;

[Serializable]
[RequireComponent(typeof(BoxCollider))]
public class Tile : MonoBehaviour
{
    public int X;

    public int Y;

    public DiscoveryState DiscoveryState;

    public bool Walkable;

    public override string ToString() => $"({X} | {Y})";

    private void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }
}

public enum DiscoveryState
{
    Hidden,
    Discovered,
    Visible
}
