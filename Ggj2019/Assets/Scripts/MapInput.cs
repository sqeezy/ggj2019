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

	public Tile[] MockWayPointList;

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f, TileLayerMask))
			{
				var clickedTile = hit.collider.GetComponent<Tile>();
				if (SelectedTile == null || SelectedTile != clickedTile)
				{
					Debug.Log("SelectedTile");
					SelectedTile = clickedTile;
					
					//TODO: highlight path from WalkOnGrid
				}
				else // selected tile == clicked tile
				{
					Debug.Log("Move");
					//TODO: Confirm / trigger movement.
					//TODO: get path from walk on grid
					//TODO: give player controller tile-list
					PlayerMovement.MoveOnPath(MockWayPointList);
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