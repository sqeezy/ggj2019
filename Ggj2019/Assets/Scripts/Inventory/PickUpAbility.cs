using UnityEngine;

[RequireComponent(typeof(PlayerActor))]
public class PickUpAbility : Ability
{
	public override void Do(GameObject targetPickupable)
	{
		if (GetComponent<PlayerActor>().CarriedPickupableActor == null)
		{
			var pickupable = targetPickupable.GetComponent<PickupableActor>();
			if (pickupable != null)
			{
				pickupable.PositionTile.Walkable = true;
				pickupable.gameObject.SetActive(false);
				GetComponent<PlayerActor>().CarriedPickupableActor = pickupable;

				//TODO: hide gameobject, make tile walkable, trigger animation...
			}
		}
	}
}