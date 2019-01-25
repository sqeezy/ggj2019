using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkOnGrid
{
    public IEnumerable<Tile> GetPath(Tile[,] grid, Tile start, Tile target)
    {
        return new[] {start};
    }
}