using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

public class MapEditor : EditorWindow
{
	[MenuItem("GGJ2019/MapEditor")]
	public static void OpenWindow()
	{
		var window = GetWindow<MapEditor>();
		window.Show();
	}

	private static int _xWidth;
	private static int _yWidth;
	private Tile _basicTile;
	private int _numberOfTiles;

	private static Tile[,] _activeGrid;

	private string _newSetName;
	
	public static string ActiveSet { get;private set; }
	public static Dictionary<string, List<Tile>> TileSets = new Dictionary<string, List<Tile>>();
	private static Map _map;

	void OnGUI()
	{
		_basicTile = EditorGUILayout.ObjectField("BaseTile", _basicTile, typeof(Tile), false) as Tile;
		_xWidth = EditorGUILayout.IntField("XWidth", _xWidth);
		_yWidth = EditorGUILayout.IntField("YWidth", _yWidth);
		if (GUILayout.Button("BuildMap"))
		{
			BuildMap();
		}

		foreach (var setId in TileSets.Keys)
		{
			var oldColor = GUI.backgroundColor;
			if (ActiveSet == setId)
			{
				GUI.backgroundColor = Color.green;	
			}

			if (GUILayout.Button(setId))
			{
				ActiveSet = setId;
			}

			GUI.backgroundColor = oldColor;
		}
		
		GUILayout.BeginHorizontal();
		_newSetName = EditorGUILayout.TextField("NewSetName:", _newSetName);
		if (GUILayout.Button("Create Set"))
		{
			var selectedTiles = new List<Tile>();
			foreach (var selectedObject in Selection.objects)
			{
				var go = selectedObject as GameObject;
				if (go != null)
				{
					Debug.Log("Found GO: " +go.name);
					var tile = go.GetComponent<Tile>();
					if (tile != null)
					{
						Debug.Log("Found tile: " + tile.name);
						selectedTiles.Add(tile);
					}
				}
			}
			TileSets.Add(_newSetName, selectedTiles);
		}

		GUILayout.EndHorizontal();
		

		if (GUILayout.Button("SetWalkable"))
		{
			foreach (var selectedObject in TileEditor._selectedObjects)
			{
				selectedObject.Walkable = true;
			}
		}
		
		if (GUILayout.Button("Set Not Walkable"))
		{
			foreach (var selectedObject in TileEditor._selectedObjects)
			{
				selectedObject.Walkable = false;
			}
		}

		if (_map == null)
		{
			GUILayout.Label("No Map loaded!!");
		}
		if (GUILayout.Button("Laod Map"))
		{
			_map = GameObject.FindObjectOfType<Map>();
			_map.Load();
			_activeGrid = _map.Grid;
		}

		if(GUILayout.Button("Validate")){
			
			var tiles = _map.GetComponentsInChildren<Tile>();
			var testGrid = new Tile[_xWidth,_yWidth];
			var selectedTile = new List<GameObject>();
			foreach(var tile in tiles)
			{
				if(testGrid[tile.X, tile.Y] != null)
				{
					selectedTile.Add(tile.gameObject);
				} else {
					testGrid[tile.X, tile.Y] = tile;
				}
			}
			Selection.objects = selectedTile.ToArray();
		}

	}

	private void BuildMap()
	{
		_map = new GameObject("Map").AddComponent<Map>();
		_activeGrid = BuildGrid(_map);

		StoreMap();
	}

	public static void ReplaceTile(Tile oldTile, Tile newTile)
	{
		//_activeGrid[oldTile.X, oldTile.Y] = newTile;
		//StoreMap();
	}

	public static void StoreMap()
	{
		_map.Store(_activeGrid, _xWidth, _yWidth);
	}

	private Tile[,] BuildGrid(Map parent)
	{
		var grid = new Tile[_xWidth, _yWidth];
		for (var xIndex = 0; xIndex < _xWidth; xIndex++)
		{
			for (var yIndex = 0; yIndex < _yWidth; yIndex++)
			{
				var tile = PrefabUtility.InstantiatePrefab(_basicTile as Tile) as Tile;
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