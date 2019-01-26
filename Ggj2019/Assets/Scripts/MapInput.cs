// File: PlayerController.cs
// Project: Divisor - HAW Math Games 
// Author: Konstantin Rudolph
// Creation Date: 22:02, 25.01.2019
// Last Edit: 22:21, 25.01.2019 by Konstantin Rudolph

using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MapInput : MonoBehaviour
{
	public LayerMask InputLayerMask;

	public event Action<GameObject> GameObjectClicked;

	public event Action<GameObject> GameObjectPushActionCalled;

	public event Action<GameObject> GameObjectPickUpActionCalled;
	
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
				GameObjectPushActionCalled.Raise(hit.collider.gameObject);
			}
		}

		if (Input.GetMouseButtonDown(2))
		{
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, 100f, InputLayerMask))
			{
				GameObjectPickUpActionCalled.Raise(hit.collider.gameObject);
			}
		}
	}
}