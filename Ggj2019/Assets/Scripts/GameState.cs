using System;
using UnityEngine;

public class GameState : MonoBehaviour
{
	public event Action<Tile, Tile> SelectedTileChanged;
	
	public PlayerActor ActiveActor;
	public MapInput Input;
	public Map MapState;
	public MainUI UI;

	public int StartEnergy;
	public int CurrentEnergy { get; set; }

	private Tile _selectedTile;

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
		Input.GameObjectPickupActionCalled += InputOnGameObjectPickupActionCalled;
		Input.DropActionCalled += InputDropActionCalled;
		CurrentEnergy = StartEnergy;
	}

	private void InputDropActionCalled()
	{
		if (ActiveActor == null)
		{
			return;
		}

		ActiveActor.ActivateTertiaryAbility(ActiveActor, ActiveActor.PositionTile.gameObject);
	}

	
	private void ReduceEnergy(int amount)
	{
		CurrentEnergy -= amount;
		UI.SetShipBar(CurrentEnergy);
	}

	private void InputOnGameObjectPickupActionCalled(GameObject obj)
	{
		if (ActiveActor == null)
		{
			return;
		}

		ActiveActor.ActivateSecondaryAbility(ActiveActor, obj);
	}

	private void InputOnGameObjectClicked(GameObject obj)
	{
		if (obj.GetComponent<Tile>() is Tile tile)
		{
			if (ActiveActor == null)
			{
				return;
			}
			/* //Confirmation.
			if (SelectedTile == tile)
			{
				ActiveActor.TargetConfirmed(tile);
			}
			else
			{
				SelectedTile = tile;
				ActiveActor.TargetClicked(tile);
			}
			*/
			ActiveActor.TargetClicked(tile);
			ActiveActor.TargetConfirmed(tile);

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
		else if (obj.GetComponent<PushableActor>())
		{
			if (ActiveActor == null)
			{
				return;
			}

			ActiveActor.ActivatePrimaryAbility(ActiveActor, obj);
		}
	}

	
}