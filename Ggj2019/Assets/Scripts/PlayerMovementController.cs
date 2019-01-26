using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
	private int NextWayPointIndex;
	public float Speed;
	private Tile[] WaypointList;
	public bool HasPath { get; private set; }

	// Update is called once per frame
	private void Update()
	{
		if (HasPath)
		{
			var nextPoint = WaypointList[NextWayPointIndex];
			var playerPosition = new Vector2(transform.position.x, transform.position.y);
			var targetPosition = new Vector2(nextPoint.X, nextPoint.Y);
			var moveDir = (targetPosition - playerPosition).normalized;
			var moveVec = moveDir * Speed * Time.deltaTime;

			//var nextPos = playerPosition
			if (moveVec.sqrMagnitude > (targetPosition - playerPosition).sqrMagnitude)
			{
				var distOverflow = moveVec.magnitude - (targetPosition - playerPosition).magnitude;
				//travel to next point if there is one with rest dist.
				if (NextWayPointIndex + 1 < WaypointList.Length)
				{
					NextWayPointIndex++;
					var oldTargetPosition = targetPosition;
					nextPoint = WaypointList[NextWayPointIndex];
					targetPosition = new Vector2(nextPoint.X, nextPoint.Y);
					moveDir = (targetPosition - oldTargetPosition).normalized;
					moveVec = moveDir * distOverflow;
					transform.position = transform.position = oldTargetPosition + moveVec;
				}
				else
				{
					transform.position = targetPosition;
					HasPath = false;
				}
			}
			else if (moveVec.magnitude <= 0.005f)
			{
				if (NextWayPointIndex + 1 < WaypointList.Length)
				{
					NextWayPointIndex++;
					var oldTargetPosition = targetPosition;
					nextPoint = WaypointList[NextWayPointIndex];
					targetPosition = new Vector2(nextPoint.X, nextPoint.Y);
					moveDir = (targetPosition - oldTargetPosition).normalized;
					moveVec = moveDir * Speed * Time.deltaTime;
					transform.position = transform.position = oldTargetPosition + moveVec;
				}
				else
				{
					transform.position = targetPosition;
					HasPath = false;
				}
			}
			else
			{
				transform.position = transform.position + new Vector3(moveVec.x, moveVec.y, 0.0f);
			}
		}
	}

	public void MoveOnPath(IEnumerable<Tile> path)
	{
		WaypointList = path.ToArray();
		if (WaypointList.Length > 0)
		{
			NextWayPointIndex = 0;
			HasPath = true;
		}
		else
		{
			HasPath = false;
		}
	}
}