using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableActor : PlayerMovementController
{
	public bool BlocksMovement;
	private bool OriginalWalkable;

	protected override void Start()
	{
		base.Start();
		OriginalWalkable = PositionTile.Walkable;
		PositionTile.Walkable = !BlocksMovement;
	}

	public void Push(Vector3 direction)
	{
		var target = new Vector2Int((int)PositionTile.X + (int)direction.x, (int)PositionTile.Y+(int)direction.y);
		var targetTile = WalkOnGrid.Grid[target.x, target.y];
		TargetClicked(targetTile);
		TargetConfirmed(targetTile);
	}

	protected override void UpdateTile(Tile nextPoint)
	{
		PositionTile.Walkable = OriginalWalkable;
		base.UpdateTile(nextPoint);

		PositionTile.Walkable = !BlocksMovement;
	}

	public override void TargetConfirmed(Tile tile)
	{
		var pushableItemOnTarget = false; // get pushable on tile
		base.TargetConfirmed(tile);
		// coroutine -> reached adjacent tile -> push
	}
}