using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int X;

    public int Y;

    public DiscoveryState DiscoveryState;

    public bool Walkable;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}

public enum DiscoveryState
{
    Hidden,
    Discovered,
    Visible
}
