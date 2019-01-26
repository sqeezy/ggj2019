using System;
using System.Collections.Generic;
using UnityEngine;

public class Actor : Revealable
{
	public Ability PrimaryAbility;

	public Ability SecondaryAbility;

	public Ability TertiaryAbility;

	public WalkOnGrid WalkOnGrid;

	public IEnumerable<Tile> Path { get; protected set; }
	public event Action<int> EnergyConsumed = t => { };

	public virtual void Deselect()
	{
	}


	public virtual void TargetClicked(Tile target)
	{
	}

	public virtual void TargetConfirmed(Tile tile)
	{
	}

	protected void ConsumeEnergy(int amount)
	{
		EnergyConsumed(amount);
	}

	public void ActivatePrimaryAbility(PlayerActor activeActor, GameObject targetObject)
	{
		MoveToTargetObjectThenActivateAbility(activeActor, PrimaryAbilityOnMovementFinished, targetObject);
	}

	public void ActivateSecondaryAbility(PlayerActor activeActor, GameObject targetObject)
	{
		MoveToTargetObjectThenActivateAbility(activeActor, SecondaryAbilityOnMovementFinished, targetObject);
	}

	public void ActivateTertiaryAbility(PlayerActor activeActor, GameObject targetObject)
	{
		MoveToTargetObjectThenActivateAbility(activeActor, TertiaryAbilityOnMovementFinished, targetObject);
	}

	private void MoveToTargetObjectThenActivateAbility(PlayerActor activeActor,
	                                                   ArrivedAtTargetObjectCallback callbackOnMovementFinished,
	                                                   GameObject targetObject)
	{
		if (activeActor is PlayerMovementController movementActor)
		{
			var targetTile = activeActor.WalkOnGrid.Grid[(int) targetObject.transform.position.x,
			                                             (int) targetObject.transform.position.y];
			activeActor.TargetClicked(targetTile);
			activeActor.TargetConfirmed(targetTile);

			Action handler = null;
			handler = () =>
			          {
				          callbackOnMovementFinished(targetObject);
				          movementActor.MovementFinished -= handler;
			          };

			movementActor.MovementFinished += handler;
		}
	}

	private void PrimaryAbilityOnMovementFinished(GameObject targetObject)
	{
		if (PrimaryAbility != null)
		{
			PrimaryAbility.Do(targetObject);
			ConsumeEnergy(PrimaryAbility.EnergyAmount);
		}
	}

	private void SecondaryAbilityOnMovementFinished(GameObject targetObject)
	{
		if (SecondaryAbility != null)
		{
			SecondaryAbility.Do(targetObject);
			ConsumeEnergy(SecondaryAbility.EnergyAmount);
		}
	}

	private void TertiaryAbilityOnMovementFinished(GameObject targetObject)
	{
		if (TertiaryAbility != null)
		{
			TertiaryAbility.Do(targetObject);
			ConsumeEnergy(TertiaryAbility.EnergyAmount);
		}
	}

	private delegate void ArrivedAtTargetObjectCallback(GameObject targetObject);
}