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
		var dropPosition = targetTile.GetComponent<Tile>();
		var carriedActor = carriedPickupableActor.GetComponent<PlayerActor>();
		if (carriedActor != null)
		{
			if (carriedActor.IsRobot && dropPosition.GetComponent<HomeArea>() != null)
			{
				carriedActor.StopMovement();
				carriedActor.RefillToFull();
			}
		}

		var pickupable = carriedPickupableActor;
		dropPosition.Walkable = !pickupable.BlocksMovement;
        carriedPickupableActor.SetPosition(dropPosition);
		carriedPickupableActor.Drop();

		playerActor.CarriedPickupableActor = null;
		AnimationController.Reset();
		AnimationController.Idle();
	}
}