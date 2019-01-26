using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public enum UpgradeState
{
	NoUpgrade,
	Upgrade1,
	Upgrade2
}
public class PlayerActor : PlayerMovementController
{

	public int FullEnergy;
	public int MaxEnergy;
	public int CurrentEnergy;
	public PickupableActor CarriedPickupableActor = null;
	public CharacterAnimation AnimationController;
	public UpgradeState ActiveUpgrade;

	public bool ForceUpgrade;

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
