using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Map))]
[CanEditMultipleObjects]
public class TileEditor : Editor
{
	public enum PaintMode
	{
		None,
		Additive, 
		Subtract
	}

	private PaintMode _mode;

	public static HashSet<Tile> _selectedObjects = new HashSet<Tile>();

	void OnEnable()
	{
		ClearSelection();

		SceneView.onSceneGUIDelegate += this.OnSceneMouseOver;
		Selection.selectionChanged += ClearSelection;
	}

	private void ClearSelection()
	{
		foreach (var tile in FindObjectsOfType<Tile>())
		{
			var renderer = tile.GetComponent<SpriteRenderer>();
			if (renderer != null)
			{
				renderer.color = Color.white;
			}
		}

		_selectedObjects.Clear();
		_mode = PaintMode.None;
	}


	void OnSceneMouseOver(SceneView view)
	{
		Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
		RaycastHit hit;
		//And add switch Event.current.type for checking Mouse click and switch tiles
		if (Physics.Raycast (ray, out hit, 100f)) 
		{
			//Debug.DrawRay (ray.origin, hit.transform.position, Color.blue, 5f);
			var tile = hit.collider.GetComponent<Tile>();
			
			if (tile != null && _mode == PaintMode.Additive)
			{
				if (!_selectedObjects.Contains(tile))
				{
					_selectedObjects.Add(tile);
					var renderer = tile.GetComponent<SpriteRenderer>();
					if (renderer != null)
					{
						renderer.color = Color.cyan;
					}
				}
				//Debug.Log (string.Format("Tile: {0}:{1}", tile.X, tile.Y));
			}

			if (tile != null && _mode == PaintMode.Subtract)
			{
				if (_selectedObjects.Contains(tile))
				{
					_selectedObjects.Remove(tile);
					var renderer = tile.GetComponent<SpriteRenderer>();
					if (renderer != null)
					{
						renderer.color = Color.white;
					}
				}
			}
		}
	}
	
	void OnSceneGUI()
	{
		Event e = Event.current;
		switch (e.type)
		{
			case EventType.MouseDown:
				ClearSelection();
				break;
			case EventType.KeyDown:
				if (Event.current.keyCode == (KeyCode.A))
				{
					_mode = PaintMode.Additive;
				}
				else if (Event.current.keyCode == (KeyCode.S))
				{
					_mode = PaintMode.Subtract;
				}
				break;

			case EventType.KeyUp:
				if (Event.current.keyCode == KeyCode.Escape)
				{
					ClearSelection();
					return;
				}

				if (Event.current.keyCode == (KeyCode.A) || Event.current.keyCode == KeyCode.S)
				{
					_mode = PaintMode.None;
				}
				if (Event.current.keyCode == KeyCode.Space)
				{

					if (MapEditor.ActiveSet != null)
					{
						var activeList = MapEditor.TileSets[MapEditor.ActiveSet];
						if (activeList.Count == 0)
						{
							return;
						}

						foreach (var selectedTile in _selectedObjects)
						{
							var choosen = Random.Range(0, activeList.Count-1);
							var targetTile = activeList[choosen];
							var newTile = PrefabUtility.InstantiatePrefab(targetTile as Tile) as Tile;
							newTile.transform.position = selectedTile.transform.position;
							newTile.transform.SetParent(selectedTile.transform.parent);
							newTile.X = selectedTile.X;
							newTile.Y = selectedTile.Y;
							MapEditor.ReplaceTile(selectedTile, newTile);
							DestroyImmediate(selectedTile.gameObject);
							
						}
					}
					else
					{
						Debug.LogWarning("No matching tile selected");
					}
					ClearSelection();

				}

				break;
		}
	}
}