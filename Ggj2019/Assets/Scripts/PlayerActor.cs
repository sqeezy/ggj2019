public class PlayerActor : PlayerMovementController
{
	public CharacterAnimation AnimationController;
	public PickupableActor CarriedPickupableActor;
	public int CurrentEnergy;
	public int FullEnergy;
	public int MaxEnergy;

	protected override void OnHasPathUpdated()
	{
		base.OnHasPathUpdated();

		if (HasPath)
		{
			AnimationController.Move();
		}
		else
		{
			AnimationController.Idle();
		}
	}

	protected override void UpdateTile(Tile nextPoint)
	{
		base.UpdateTile(nextPoint);
		CurrentEnergy--;
	}

	public void RefillToFull()
	{
		CurrentEnergy = FullEnergy;
	}
}