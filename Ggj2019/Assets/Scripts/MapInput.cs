#region FileHeader

// File: PlayerController.cs
// Project: Divisor - HAW Math Games 
// Author: Konstantin Rudolph
// Creation Date: 22:02, 25.01.2019
// Last Edit: 22:21, 25.01.2019 by Konstantin Rudolph

#endregion

#region

using UnityEngine;

#endregion

[RequireComponent(typeof(Camera))]
public class MapInput : MonoBehaviour
{
	private readonly WalkOnGrid _walker = new WalkOnGrid();

	public GameState GlobalState;

	public PlayerMovementController PlayerMovement;

	public LayerMask TileLayerMask;

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (PlayerMovement.HasPath)
			{
				return;
			}

			RaycastHit hit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f, TileLayerMask))
			{
				var clickedTile = hit.collider.GetComponent<Tile>();
				if (GlobalState.SelectedTile == null || GlobalState.SelectedTile != clickedTile)
				{
					GlobalState.SelectedTile = clickedTile;

					//TODO: highlight path from WalkOnGrid
				}
				else // selected tile == clicked tile
				{
					//TODO: Confirm / trigger movement.
					//TODO: get path from walk on grid
					//TODO: give player controller tile-list

					var playerX = (int) PlayerMovement.transform.position.x;
					var playerY = (int) PlayerMovement.transform.position.y;
					GlobalState.MapState.Load();
					var playerTile = GlobalState.MapState.Grid[playerX, playerY];

					PlayerMovement.MoveOnPath(_walker.GetPath(GlobalState.MapState.Grid, playerTile, clickedTile));
				}
			}
		}
		else if (Input.GetMouseButtonDown(1))
		{
			// Cancels path.
			GlobalState.SelectedTile = null;
		}
	}
}