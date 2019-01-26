#region

using System;
using UnityEngine;

#endregion

public class GameState : MonoBehaviour

{
	private Tile _selectedTile;
	public object ActiveActor;
	public Map MapState;

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

	public event Action<Tile, Tile> SelectedTileChanged;
}