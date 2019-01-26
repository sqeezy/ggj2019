using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapEditor : EditorWindow
{
	[MenuItem("GGJ2019/MapEditor")]
	public static void OpenWindow()
	{
		var window = GetWindow<MapEditor>();
		window.Show();
	}

	private int _xWidth;
	private int _yWidth;
	private Tile _basicTile;
	private List<Tile> _tilesToApply = new List<Tile>();
	private int _numberOfTiles;

	void OnGUI()
	{
		_basicTile = EditorGUILayout.ObjectField("BaseTile", _basicTile, typeof(Tile), false) as Tile;
		_xWidth = EditorGUILayout.IntField("XWidth", _xWidth);
		_yWidth = EditorGUILayout.IntField("YWidth", _yWidth);
		if (GUILayout.Button("BuildMap"))
		{
			BuildMap();
		}

		_numberOfTiles = EditorGUILayout.IntField("numberOfTiles", _numberOfTiles);
		for (int i = 0; i < _numberOfTiles; i++)
		{
			if (i < _tilesToApply.Count)
			{
				_tilesToApply[i] =
					EditorGUILayout.ObjectField("BaseTile", _tilesToApply[i], typeof(Tile), false) as Tile;
			}
			else
			{
				Tile newTile = null;
				newTile = EditorGUILayout.ObjectField("BaseTile", newTile, typeof(Tile), false) as Tile;
				_tilesToApply.Add(newTile);
			}
		}

		if (GUILayout.Button("Apply to selected"))
		{
			foreach (var selectedTile in Selection.gameObjects)
			{
				var tileId = Random.Range(0, _numberOfTiles);
				var newTile = Instantiate(_tilesToApply[tileId]);
				newTile.transform.position = selectedTile.transform.position;
				newTile.transform.SetParent(selectedTile.transform.parent);
			}

			for (int i = Selection.gameObjects.Length - 1; i >= 0; i--)
			{
				DestroyImmediate(Selection.gameObjects[i]);
			}
		}

		if (GUILayout.Button("Connect"))
		{
			Connect();
		}
	}

	private void Connect()
	{
		Selection.selectionChanged = null;
		Selection.selectionChanged = UpdateSelection;
	}

	private void UpdateSelection()
	{
		var tiles = FindObjectsOfType<Tile>();
		foreach (var tile in tiles)
		{
			var sprite = tile.GetComponent<SpriteRenderer>();
			if (sprite != null)
			{
				sprite.color = Color.white;
			}
		}

		foreach (var gameObject in Selection.gameObjects)
		{
			var sprite = gameObject.GetComponent<SpriteRenderer>();
			if (sprite != null)
			{
				sprite.color = Color.cyan;
			}
		}
	}

	private void BuildMap()
	{
		var map = new GameObject("Map").AddComponent<Map>();
		var grid = BuildGrid(map);

		map.Store(grid, _xWidth, _yWidth);
	}

	private Tile[,] BuildGrid(Map parent)
	{
		var grid = new Tile[_xWidth, _yWidth];
		for (var xIndex = 0; xIndex < _xWidth; xIndex++)
		{
			for (var yIndex = 0; yIndex < _yWidth; yIndex++)
			{
				Tile tile = Instantiate(_basicTile);
				tile.transform.position = new Vector3(xIndex, yIndex, 0);
				tile.transform.SetParent(parent.transform);
				tile.X = xIndex;
				tile.Y = yIndex;
				grid[xIndex, yIndex] = tile;
			}
		}

		return grid;
	}
}