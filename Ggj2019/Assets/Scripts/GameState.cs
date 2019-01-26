#region

using System;
using UnityEngine;

#endregion

public class GameState : MonoBehaviour
{
	private Tile _selectedTile;
	public PlayerActor ActiveActor;
	public MapInput Input;
	public Map MapState;
	
	public int StartEnergy; 
	public int CurrentEnergy { get; set; }

	
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
		Input.GameObjectActionCalled += InputOnGameObjectActionCalled;
		CurrentEnergy = StartEnergy;
		ActiveActor.EnergyConsumed += ReduceEnergy;
	}

	private void ReduceEnergy(int amount)
	{
		CurrentEnergy -= amount;
	}

	private void InputOnGameObjectActionCalled(GameObject obj)
	{
		if (ActiveActor == null)
		{
			return;

		}

		ActiveActor.ActivateBasicAbility(ActiveActor, obj);

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
		else if (obj.GetComponent<PlayerActor>() is PlayerActor actor)
		{
			ActiveActor.Deselect();
			ActiveActor = actor;
		}
	}

	public event Action<Tile, Tile> SelectedTileChanged;
}