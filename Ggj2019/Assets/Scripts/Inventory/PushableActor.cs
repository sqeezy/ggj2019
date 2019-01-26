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
		OriginalWalkable = _positionTile.Walkable;
		_positionTile.Walkable = !BlocksMovement;
	}

	public void Push(Vector3 direction)
	{
		var target = new Vector2Int((int)_positionTile.X + (int)direction.x, (int)_positionTile.Y+(int)direction.y);
		var targetTile = WalkOnGrid.Grid[target.x, target.y];
		TargetClicked(targetTile);
		TargetConfirmed(targetTile);
	}

	protected override void UpdateTile(Tile nextPoint)
	{
		_positionTile.Walkable = OriginalWalkable;
		base.UpdateTile(nextPoint);

		_positionTile.Walkable = !BlocksMovement;
	}
}