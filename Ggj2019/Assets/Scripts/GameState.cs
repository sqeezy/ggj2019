using System;
using UnityEngine;

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
		Input.GameObjectPushActionCalled += InputOnGameObjectPushActionCalled;
		Input.GameObjectPickUpActionCalled += InputOnGameObjectPickUpActionCalled;
		CurrentEnergy = StartEnergy;
	}

	private void InputOnGameObjectPickUpActionCalled(GameObject obj)
	{
		if (ActiveActor == null)
		{
			return;
		}

		ActiveActor.ActivateSecondaryAbility(ActiveActor, obj);
	}

	private void ReduceEnergy(int amount)
	{
		CurrentEnergy -= amount;
	}

	private void InputOnGameObjectPushActionCalled(GameObject obj)
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
			if (ActiveActor != null)
			{
				ActiveActor.EnergyConsumed -= ReduceEnergy;
				ActiveActor.Deselect();
			}

			ActiveActor = actor;
			ActiveActor.EnergyConsumed += ReduceEnergy;
		}
	}

	public event Action<Tile, Tile> SelectedTileChanged;
}