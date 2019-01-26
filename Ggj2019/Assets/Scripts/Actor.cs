using System;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
	public event Action<int> EnergyConsumed = (t) => { };

	public Ability BasicAbility;
	
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

	public void ActivateBasicAbility(Actor activeActor, GameObject targetObject)
	{
		if (activeActor is PlayerMovementController movementActor)
		{
			var targetTile = activeActor.WalkOnGrid.Grid[(int)targetObject.transform.position.x, (int)targetObject.transform.position.y];
			activeActor.TargetClicked(targetTile);
			activeActor.TargetConfirmed(targetTile);

			//Action action = () => { movementActor.MovementFinished -= action; };
			Action handler = null;
			handler = () =>
			{
				MovementActorOnMovementFinished(movementActor, targetObject);
				movementActor.MovementFinished -= handler;
			};

			movementActor.MovementFinished += handler;
		}
	}

	private void MovementActorOnMovementFinished(PlayerMovementController movementActor, GameObject targetObject)
	{
		if (BasicAbility != null)
		{
			BasicAbility.Do(targetObject);
			ConsumeEnergy(BasicAbility.EnergyAmount);
		}
	}
}