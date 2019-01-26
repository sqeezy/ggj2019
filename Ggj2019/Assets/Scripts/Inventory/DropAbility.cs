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
		var dropPosition = targetTile.GetComponent<Tile>();
		dropPosition.Walkable = !pickupable.BlocksMovement;
		foreach (var component in playerActor.CarriedPickupableActor.gameObject.GetComponents<PlayerMovementController>())
		{
			component.SetPosition(dropPosition);
		}
		playerActor.CarriedPickupableActor = null;
	}
}