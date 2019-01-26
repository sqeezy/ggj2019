using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryAbility : MonoBehaviour
{
	public Item ActiveItem;

	public void PickItem(Item newItem)
	{
		if (ActiveItem != null)
		{
			ActiveItem = newItem;
			ActiveItem.PickUp();
		}
	}

	public void DropItem()
	{
		ActiveItem.Drop();
		ActiveItem = null;
	}
}