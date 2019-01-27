using UnityEngine;

[RequireComponent(typeof(PlayerActor))]
public class DropAbility : Ability
{
	public CharacterAnimation AnimationController;
	
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
        carriedPickupableActor.SetPosition(dropPosition);
		carriedPickupableActor.Drop();
		playerActor.CarriedPickupableActor = null;
		AnimationController.Reset();
		AnimationController.Idle();
	}
}