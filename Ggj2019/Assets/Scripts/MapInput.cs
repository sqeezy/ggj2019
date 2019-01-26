#region FileHeader

// File: PlayerController.cs
// Project: Divisor - HAW Math Games 
// Author: Konstantin Rudolph
// Creation Date: 22:02, 25.01.2019
// Last Edit: 22:21, 25.01.2019 by Konstantin Rudolph

#endregion

#region

using System;
using UnityEngine;

#endregion

[RequireComponent(typeof(Camera))]
public class MapInput : MonoBehaviour
{
	public LayerMask TileLayerMask;

	public event Action<GameObject> GameObjectClicked;

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, 100f, TileLayerMask))
			{
				GameObjectClicked.Raise(hit.collider.gameObject);
			}
		}
	}
}