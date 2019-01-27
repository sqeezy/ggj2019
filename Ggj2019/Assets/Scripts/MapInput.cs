﻿using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MapInput : MonoBehaviour
{
	public LayerMask InputLayerMask;

	public event Action<GameObject> GameObjectClicked;

	public event Action<GameObject> GameObjectPushActionCalled;

	public event Action<GameObject> GameObjectPickUpActionCalled;

	public event Action DropActionCalled;

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

		if (Input.GetKeyDown(KeyCode.Space))
		{
			DropActionCalled.Raise();
		}
	}
}