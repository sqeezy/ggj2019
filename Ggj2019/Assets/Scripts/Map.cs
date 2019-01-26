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
		_xWidth = xWidth;
		_yWidth = yWidth;
	}

	public void Load()
	{
		Grid = new Tile[_xWidth, _yWidth];
		foreach (var childTile in GetComponentsInChildren<Tile>())
		{
			Grid[(int) childTile.transform.position.x, (int) childTile.transform.position.y] = childTile;
		}
		
	}
}