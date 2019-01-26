using System;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
	public event Action<int> EnergyConsumed = (t) => { };

	delegate void ArrivedAtTargetObjectCallback(GameObject targetObject);

	public Ability BasicAbility;

	public Ability SecondaryAbility;

	public WalkOnGrid WalkOnGrid;

	public IEnumerable<Tile> Path { get; protected set; }

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

	public void ActivateBasicAbility(PlayerActor activeActor, GameObject targetObject)
	{
		MoveToTargetObjectThenActivateAbility(activeActor, PrimaryAbilityOnMovementFinished, targetObject);
	}

	public void ActivateSecondaryAbility(PlayerActor activeActor, GameObject targetObject)
	{
		MoveToTargetObjectThenActivateAbility(activeActor, SecondaryAbilityOnMovementFinished, targetObject);
	}

	private void MoveToTargetObjectThenActivateAbility(PlayerActor activeActor, ArrivedAtTargetObjectCallback callbackOnMovementFinished, GameObject targetObject)
	{
		if (activeActor is PlayerMovementController movementActor)
		{
			var targetTile = activeActor.WalkOnGrid.Grid[(int)targetObject.transform.position.x, (int)targetObject.transform.position.y];
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
		if (BasicAbility != null)
		{
			BasicAbility.Do(targetObject);
			ConsumeEnergy(BasicAbility.EnergyAmount);
		}
	}

	private void SecondaryAbilityOnMovementFinished(GameObject targetObject)
	{
		if (SecondaryAbility != null)
		{
			SecondaryAbility.Do(targetObject);
			ConsumeEnergy((SecondaryAbility.EnergyAmount));
		}
	}
}