using System;
using System.Linq;
using UnityEngine;

public class PlayerMovementController : Actor
{
	private bool _hasPath;
	private (int, int) _position;

	public bool BlocksMovement;
	private int NextWayPointIndex;
	public float Speed;
	public Tile StartPosition;
	private Tile[] WaypointList;
	public Tile PositionTile { get; protected set; }

	public bool HasPath
	{
		get => _hasPath;
		private set
		{
			_hasPath = value;
			OnHasPathUpdated();
		}
	}

	public event Action MovementFinished;

	protected virtual void OnHasPathUpdated()
	{
	}


	protected virtual void Start()
	{
		base.Start();
		var pos = StartPosition.transform.position;
		pos.z = -1f;
		transform.position = pos;
		PositionTile = StartPosition;
		PositionTile.Walkable = !BlocksMovement;
	}

	public void SetPosition(Tile newPosition)
	{
		transform.position = new Vector3(newPosition.X, newPosition.Y, -1f);
		PositionTile = newPosition;
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
		var scale = transform.localScale;
		if (moveDir.x < 0)
		{
			scale.x = -1;
		}
		else
		{
			scale.x = 1;
		}

		transform.localScale = scale;

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
				var tmp = oldTargetPosition + moveVec;
				transform.position = new Vector3(tmp.x, tmp.y, -1f);
			}
			else
			{
				FinishMovement(targetPosition);
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
				var tmp = oldTargetPosition + moveVec;
				transform.position = new Vector3(tmp.x, tmp.y, -1f);
			}
			else
			{
				FinishMovement(targetPosition);
			}
		}
		else
		{
			var tmp = transform.position + new Vector3(moveVec.x, moveVec.y, 0.0f);
			tmp.z = -1.0f;
			transform.position = tmp;
		}
	}

	private void FinishMovement(Vector2 targetPosition)
	{
		var tmp = new Vector3(targetPosition.x, targetPosition.y);
		tmp.z = -1f;
		transform.position = tmp;
		HasPath = false;
		MovementFinished.Raise();
	}

	protected virtual void UpdateTile(Tile nextPoint)
	{
		PositionTile = nextPoint;
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
		Path = WalkOnGrid.GetPath(PositionTile, target);
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