#region

using System.Collections.Generic;
using UnityEngine;

#endregion

[RequireComponent(typeof(PlayerMovementController))]
public class Actor : MonoBehaviour
{
	public WalkOnGrid WalkOnGrid;
	private PlayerMovementController _movement;

	private void Start()
	{
		_movement = GetComponent<PlayerMovementController>();
	}

	public IEnumerable<Tile> Path { get; private set; }

	public void TargetClicked(Tile target)
	{
		Tile position = new Tile();
		Path = WalkOnGrid.GetPath(position, target);
	}

	public void TargetConfirmed(Tile tile)
	{
		_movement.MoveOnPath(Path);
	}
}