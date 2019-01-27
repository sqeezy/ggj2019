﻿using UnityEngine;

public class PushableActor : PlayerMovementController
{
	public Animator AnimationController;
	public void Push(Vector3 direction) 
	{
		var directionNormalized = direction.normalized;
		var target = new Vector2Int(PositionTile.X + (int) directionNormalized.x, PositionTile.Y + (int) directionNormalized.y);
		var targetTile = WalkOnGrid.Grid[target.x, target.y];
		TargetClicked(targetTile);
		TargetConfirmed(targetTile);
	}

	protected override void UpdateTile(Tile nextPoint)
	{
		PositionTile.Walkable = true;
		base.UpdateTile(nextPoint);

		PositionTile.Walkable = !BlocksMovement;
	}

	public override void TargetConfirmed(Tile tile)
	{
		if (AnimationController != null)
		{
			AnimationController.SetTrigger("Move");
		}

		base.TargetConfirmed(tile);
		
		// coroutine -> reached adjacent tile -> push
	}
}