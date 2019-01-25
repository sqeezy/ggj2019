using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

	private int xWidth;
	private int yWidth;
	private Tile basicTile;
	private List<Tile> tilesToApply = new List<Tile>();
	private int numberOfTiles;

	void OnGUI()
	{
		basicTile = EditorGUILayout.ObjectField("BaseTile", basicTile, typeof(Tile), false) as Tile;
		xWidth = EditorGUILayout.IntField("XWidth", xWidth);
		yWidth = EditorGUILayout.IntField("YWidth", yWidth);
		if (GUILayout.Button("BuildMap"))
		{
			BuildMap();
		}
		
		numberOfTiles = EditorGUILayout.IntField("numberOfTiles", numberOfTiles);
		for (int i = 0; i < numberOfTiles; i++)
		{
			if (i < tilesToApply.Count)
			{
				tilesToApply[i] = EditorGUILayout.ObjectField("BaseTile", tilesToApply[i], typeof(Tile), false) as Tile;
			}
			else
			{
				Tile newTile = null; 
				newTile = EditorGUILayout.ObjectField("BaseTile", newTile, typeof(Tile), false) as Tile;
				tilesToApply.Add(newTile);
			}
		}

		if (GUILayout.Button("Apply to selected"))
		{
			var tile = Random.Range(0, numberOfTiles);
			foreach (var selectedTile in Selection.gameObjects)
			{
				var newTile = Instantiate(tilesToApply[tile]);
				newTile.transform.position = selectedTile.transform.position;
			}

			for (int i = Selection.gameObjects.Length - 1; i >= 0; i--)
			{
				DestroyImmediate(Selection.gameObjects[i]);
			}
		}

		if (GUILayout.Button("Connect"))
		{
			Selection.selectionChanged += Connect;
		}
	}

	private void Connect()
	{
		Selection.selectionChanged = null;
		Selection.selectionChanged += UpdateSelection;
	}

	private void UpdateSelection()
	{
		var tiles = FindObjectsOfType<Tile>();
		foreach (var tile in tiles)
		{
			var sprite = tile.GetComponent<SpriteRenderer>();
			sprite.color = Color.white;
		}

		foreach (var gameObject in Selection.gameObjects)
		{
			var sprite = gameObject.GetComponent<SpriteRenderer>();
			sprite.color = Color.cyan;
		}
	}

	private void BuildMap()
	{
		var grid = new Tile[xWidth, yWidth];
		for (var xIndex = 0; xIndex < xWidth; xIndex++)
		{
			for (var yIndex = 0; yIndex < yWidth; yIndex++)
			{
				Tile tile = Instantiate(basicTile);
				tile.transform.position = new Vector3(xIndex, yIndex, 0);
				grid[xIndex, yIndex] = tile;
			}
		}
	}
}