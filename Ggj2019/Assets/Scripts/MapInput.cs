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
	public LayerMask InputLayerMask;

	public event Action<GameObject> GameObjectClicked;

	public event Action<GameObject> GameObjectActionCalled;

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, 100f, InputLayerMask))
			{
				GameObjectClicked.Raise(hit.collider.gameObject);
			}
		}

		if (Input.GetMouseButtonDown(1))
		{
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, 100f, InputLayerMask))
			{
				GameObjectActionCalled.Raise(hit.collider.gameObject);
			}
		}
	}
}