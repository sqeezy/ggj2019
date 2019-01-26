using System;
using UnityEngine;

[Serializable]
public class Map : MonoBehaviour
{
	[SerializeField] private Tile[] _flattenedGrid;
	[SerializeField] private int _xWidth;
	[SerializeField] private int _yWidth;
	public Tile[,] Grid;

	public void Store(Tile[,] grid, int xWidth, int yWidth)
	{
		_xWidth = xWidth;
		_yWidth = yWidth;
	}

	public void Load()
	{
		Grid = new Tile[_xWidth, _yWidth];
		foreach (var childTile in GetComponentsInChildren<Tile>())
			Grid[(int) childTile.transform.position.x, (int) childTile.transform.position.y] = childTile;
	}
}