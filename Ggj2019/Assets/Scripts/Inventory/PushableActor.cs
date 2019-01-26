using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableActor : PlayerMovementController
{
	public void Push(Vector3 direction)
	{
		var target = new Vector2Int((int)_positionTile.X + (int)direction.x, (int)_positionTile.Y+(int)direction.y);
		var targetTile = WalkOnGrid.Grid[target.x, target.y];
		TargetClicked(targetTile);
		TargetConfirmed(targetTile);
	}
}