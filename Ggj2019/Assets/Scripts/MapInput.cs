#region FileHeader

// File: PlayerController.cs
// Project: Divisor - HAW Math Games 
// Author: Konstantin Rudolph
// Creation Date: 22:02, 25.01.2019
// Last Edit: 22:21, 25.01.2019 by Konstantin Rudolph

#endregion

using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MapInput : MonoBehaviour
{
	public LayerMask TileLayerMask;

	private Tile SelectedTile;

	public PlayerMovementController PlayerMovement;

	public Map ActiveMap;

	private WalkOnGrid _walker = new WalkOnGrid();

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (PlayerMovement.HasPath)
			{
				// Early out, in case the character is already moving.
				return;
			}

			RaycastHit hit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f, TileLayerMask))
			{
				var clickedTile = hit.collider.GetComponent<Tile>();
				if (SelectedTile == null || SelectedTile != clickedTile)
				{
					SelectedTile = clickedTile;

					//TODO: highlight path from WalkOnGrid
				}
				else // selected tile == clicked tile
				{
					//TODO: Confirm / trigger movement.
					//TODO: get path from walk on grid
					//TODO: give player controller tile-list

					var playerX = (int) PlayerMovement.transform.position.x;
					var playerY = (int) PlayerMovement.transform.position.y;
					ActiveMap.Load();
					var playerTile = ActiveMap.Grid[playerX, playerY];

					PlayerMovement.MoveOnPath(_walker.GetPath(ActiveMap.Grid, playerTile, clickedTile));
				}
			}
		}
		else if (Input.GetMouseButtonDown(1))
		{
			// Cancels path.
			SelectedTile = null;
		}
	}
}