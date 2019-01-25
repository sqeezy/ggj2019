#region FileHeader

// File: PlayerController.cs
// Project: Divisor - HAW Math Games 
// Author: Konstantin Rudolph
// Creation Date: 22:02, 25.01.2019
// Last Edit: 22:21, 25.01.2019 by Konstantin Rudolph

#endregion

using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MapInput : MonoBehaviour
{
	public LayerMask TileLayerMask;

	private Tile SelectedTile;

	public PlayerMovementController PlayerMovement;

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
					SelectedTile = clickedTile;
					//TODO: highlight path from WalkOnGrid
				}
				else // selected tile == clicked tile
				{
					//TODO: Confirm / trigger movement.
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