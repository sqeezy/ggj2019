using System;
using System.Collections.Generic;

public static class Extensions
{
	private static readonly (int, int)[] _directions = {(1, 0), (0, 1), (-1, 0), (0, -1)};

	public static IEnumerable<T> Flatten<T>(this T[,] grid)
	{
		foreach (T t in grid)
		{
			yield return t;
		}
	}

	public static IEnumerable<Tile> GetNeighbors(this Tile tile, Tile[,] grid)
	{
		foreach (var (x, y) in _directions)
		{
			var newX = tile.X + x;
			var newY = tile.Y + y;
			var xInBounds = newX >= 0 && newX < grid.GetLength(0);
			var yInBounds = newY >= 0 && newY < grid.GetLength(1);
			if (xInBounds && yInBounds && grid[newX, newY].Walkable)
			{
				yield return grid[newX, newY];
			}
		}
	}

	public static void Raise(this Action a)
	{
		a?.Invoke();
	}

	public static void Raise<T>(this Action<T> a, T arg)
	{
		a?.Invoke(arg);
	}

	public static void Raise<T1,T2>(this Action<T1, T2> a, T1 arg1, T2 arg2)
	{
		a?.Invoke(arg1, arg2);
	}
}