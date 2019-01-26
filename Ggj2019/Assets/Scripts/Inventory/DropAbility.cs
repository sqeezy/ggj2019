using UnityEngine;

[RequireComponent(typeof(PlayerActor))]
public class DropAbility : Ability
{
	public override void Do(GameObject targetTile)
	{
		var playerActor = GetComponent<PlayerActor>();
		var carriedPickupableActor = playerActor.CarriedPickupableActor;
		if (carriedPickupableActor == null)
		{
			return;
		}

		var pickupable = carriedPickupableActor;
		pickupable.SetPosition(targetTile.GetComponent<Tile>());
		playerActor.CarriedPickupableActor = null;
	}
}