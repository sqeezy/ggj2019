#region

using System.Linq;
using UnityEngine;

#endregion

public class PlayerMovementController : Actor
{
	private (int, int) _position;
	protected Tile _positionTile;
	private int NextWayPointIndex;
	public float Speed;
	public Tile StartPosition;
	private Tile[] WaypointList;
	public bool HasPath { get; private set; }

	protected virtual void Start()
	{
		var pos = StartPosition.transform.position;
		pos.z = -1f;
		transform.position = pos;
		_positionTile = StartPosition;
	}

	// Update is called once per frame
	private void Update()
	{
		if (!HasPath)
		{
			return;
		}

		var nextPoint = WaypointList[NextWayPointIndex];
		var playerPosition = new Vector2(transform.position.x, transform.position.y);
		var targetPosition = new Vector2(nextPoint.X, nextPoint.Y);
		var moveDir = (targetPosition - playerPosition).normalized;
		var moveVec = moveDir * Speed * Time.deltaTime;

		if (moveVec.sqrMagnitude > (targetPosition - playerPosition).sqrMagnitude)
		{
			UpdateTile(nextPoint);
			var distOverflow = moveVec.magnitude - (targetPosition - playerPosition).magnitude;
			//travel to next point if there is one with distance overflow.
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
			UpdateTile(nextPoint);
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

	protected virtual void UpdateTile(Tile nextPoint)
	{
		_positionTile = nextPoint;
		ConsumeEnergy(1);
	}

	private void MoveOnPath()
	{
		WaypointList = Path.ToArray();
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

	public override void Deselect()
	{
		/*
		foreach (var tile in Path)
		{
			HighlightPath(false);
		}
		*/
	}

	public override void TargetClicked(Tile target)
	{
		HighlightPath(false);
		Path = WalkOnGrid.GetPath(_positionTile, target);
		HighlightPath(true);

	}

	private void HighlightPath(bool highlight)
	{
		/*
		foreach (var tile in Path)
		{
			//TODO: highlight tile if tile is visible.
			//tile.highlight(highlight);

		}
		*/
	}

	public override void TargetConfirmed(Tile tile)
	{
		MoveOnPath();
	}
}