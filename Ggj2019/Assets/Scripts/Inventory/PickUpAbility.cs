using System.Xml.Xsl;
using UnityEngine;

[RequireComponent(typeof(PlayerActor))]
public class PickUpAbility : Ability
{
	public CharacterAnimation AnimationController;
	
	public override void Do(GameObject targetPickupable)
	{
		if (GetComponent<PlayerActor>().CarriedPickupableActor == null)
		{
			var pickupable = targetPickupable.GetComponent<PickupableActor>();
			if ((pickupable.transform.position - transform.position).magnitude > 1f)
			{
				return;
			}
			if (pickupable != null && pickupable.gameObject != gameObject)
			{
				pickupable.PositionTile.Walkable = true;
				pickupable.PickUp();
				GetComponent<PlayerActor>().CarriedPickupableActor = pickupable;
				AnimationController.AnimationData.Move = pickupable.CarryAnimationName;
				AnimationController.AnimationData.Idle = pickupable.IdleAnimationName;
				AnimationController.Idle();
			}
		}
	}
}