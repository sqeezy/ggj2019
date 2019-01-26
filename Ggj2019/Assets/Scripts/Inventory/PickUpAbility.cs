using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerActor))]
public class PickUpAbility : Ability
{

	public override void Do(GameObject targetPickupable)
	{
		if (GetComponent<PlayerActor>().CarriedPickupableActor == null)
		{
			PickupableActor pickupable = targetPickupable.GetComponent<PickupableActor>();
			if (pickupable != null)
			{
				GetComponent<PlayerActor>().CarriedPickupableActor = pickupable;
				//TODO: hide gameobject, make tile walkable, trigger animation...
			}
		}
	}
}