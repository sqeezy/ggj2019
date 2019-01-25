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
	private Tile tilePrefab; 
	
	void OnGUI()
	{
		tilePrefab = EditorGUILayout.ObjectField("BaseTile", tilePrefab, typeof(Tile), false) as Tile;
		xWidth = EditorGUILayout.IntField("XWidth", xWidth);
		yWidth = EditorGUILayout.IntField("YWidth", yWidth);
		if (GUILayout.Button("BuildMao"))
		{
			BuildMap();
		}
	}

	private void BuildMap()
	{
		var grid = new Tile[xWidth, yWidth];
		for (var xIndex = 0; xIndex < xWidth; xIndex++)
		{
			for (var yIndex = 0; yIndex < yWidth; yIndex++)
			{
				Tile tile = Instantiate(tilePrefab);
				tile.transform.position = new Vector3(xIndex, yIndex, 0);
				grid[xIndex, yIndex] = tile;
			}
		}
	}
}