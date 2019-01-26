public enum UpgradeState
{
	NoUpgrade,
	Upgrade1,
	Upgrade2
}

public class PlayerActor : PlayerMovementController
{
	public UpgradeState ActiveUpgrade;
	public CharacterAnimation AnimationController;
	public PickupableActor CarriedPickupableActor;
	public int CurrentEnergy;

	public bool ForceUpgrade;
	public int FullEnergy;
	public int MaxEnergy;

	private void FixedUpdate()
	{
		if (ForceUpgrade)
		{
			Upgrade();
			ForceUpgrade = false;
		}
	}

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

	public void Upgrade()
	{
		switch (ActiveUpgrade)
		{
			case UpgradeState.NoUpgrade:
				AnimationController.Upgrade();
				ActiveUpgrade = UpgradeState.Upgrade1;
				break;
			case UpgradeState.Upgrade1:
				AnimationController.Upgrade();
				ActiveUpgrade = UpgradeState.Upgrade2;
				break;
			case UpgradeState.Upgrade2:
				break;
		}
	}
}