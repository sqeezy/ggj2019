using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Map : MonoBehaviour
{
	public Tile[,] Grid;

	[SerializeField] private Tile[] _flattenedGrid;
	[SerializeField] private int _xWidth;
	[SerializeField] private int _yWidth;

	public void Store(Tile[,] grid, int xWidth, int yWidth)
	{
		_flattenedGrid = grid.Flatten().ToArray();
		_xWidth = xWidth;
		_yWidth = yWidth;
	}

	public void Load()
	{
		Grid = new Tile[_xWidth, _yWidth];
		for (int x = 0; x < _xWidth; x++)
		{
			for (int y = 0; y < _yWidth; y++)
			{
				Grid[x, y] = _flattenedGrid[x * _xWidth + y];
			}
		}
	}
}