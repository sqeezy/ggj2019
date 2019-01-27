using System;
using UnityEngine;

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
	public GameObject Blizzard;
	public int CurrentEnergy;
	public bool ForceUpgrade;
	public int FullEnergy;

	public bool IsRobot;
	public int MaxEnergy;

	public MainUI UI;
	public event Action EnteredHomeWithUpgrade;

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
		if (nextPoint != PositionTile)
		{
			CurrentEnergy--;
			if (CurrentEnergy <= 0)
			{
				HasPath = false;
				WaypointList = new Tile[0];
				StopAllCoroutines();
				enabled = false;
			}
		}

		base.UpdateTile(nextPoint);
		if (nextPoint.GetComponent<HomeArea>() != null)
		{
			Blizzard.SetActive(false);
			RefillToFull();

			if (CarriedPickupableActor != null && CarriedPickupableActor.UpgradesPlayerActors)
			{
				var tmp = CarriedPickupableActor;
				Destroy(tmp.gameObject);
				AnimationController.Reset();
				AnimationController.Idle();
				EnteredHomeWithUpgrade.Raise();
			}
		}
		else
		{
			Blizzard.SetActive(true);
		}

		UpdateEnergyUi();
	}

	private void UpdateEnergyUi()
	{
		if (IsRobot)
		{
			UI.SetBlueBar(CurrentEnergy, FullEnergy);
		}
		else
		{
			UI.SetRedBar(CurrentEnergy, FullEnergy);
		}
	}


	private void UpgradeEnergy()
	{
		FullEnergy = Math.Min(FullEnergy + 8, MaxEnergy);
	}

	public void RefillToFull()
	{
		CurrentEnergy = FullEnergy;
		enabled = true;
		UpdateEnergyUi();
	}

	public void Upgrade()
	{
		UpgradeEnergy();
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

	public void StopMovement()
	{
		HasPath = false;
	}
}