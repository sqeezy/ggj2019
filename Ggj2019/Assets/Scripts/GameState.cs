#region

using System;
using UnityEngine;

#endregion

public class GameState : MonoBehaviour

{
	private Tile _selectedTile;
	public Actor ActiveActor;
	public MapInput Input;
	public Map MapState;

	public GameObject SelectedObject;

	public Tile SelectedTile
	{
		get => _selectedTile;
		set
		{
			if (_selectedTile == value)
			{
				return;
			}

			var oldSelection = _selectedTile;
			_selectedTile = value;
			SelectedTileChanged.Raise(oldSelection, _selectedTile);
		}
	}

	private void Start()
	{
		Input.GameObjectClicked += InputOnGameObjectClicked;
	}

	private void InputOnGameObjectClicked(GameObject obj)
	{
		if (obj.GetComponent<Tile>() is Tile tile)
		{
			if (ActiveActor == null)
			{
				return;
			}

			if (SelectedTile == tile)
			{
				ActiveActor.TargetConfirmed(tile);
			}
			else
			{
				SelectedTile = tile;
				ActiveActor.TargetClicked(tile);
			}
		}
	}

	public event Action<Tile, Tile> SelectedTileChanged;
}