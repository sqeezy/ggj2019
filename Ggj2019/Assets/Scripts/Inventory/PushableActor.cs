using UnityEngine;

public class PushableActor : PlayerMovementController
{
	public void Push(Vector3 direction)
	{
		var target = new Vector2Int(PositionTile.X + (int) direction.x, PositionTile.Y + (int) direction.y);
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
		var pushableItemOnTarget = false; // get pushable on tile
		base.TargetConfirmed(tile);
		// coroutine -> reached adjacent tile -> push
	}
}