using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WalkOnGrid
{
	public IEnumerable<Tile> GetPath(Tile[,] grid, Tile start, Tile target)
	{
		if (start == target)
		{
			return new[] {start};
		}

		return new[] {start, target};
	}
}