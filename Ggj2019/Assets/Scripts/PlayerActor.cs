using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = System.Object;

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
				if (!IsRobot)
				{
					StartCoroutine(IncreaseBlizzard());
				}
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

	public IEnumerator IncreaseBlizzard()
	{
		float time = 4;
		while (time>=0)
		{
			time -= 0.05f;
			var system = Blizzard.GetComponent<ParticleSystem>();
			system.emissionRate += 1000;
			system.startSize += 0.005f;
			yield return new WaitForSeconds(0.05f);
			
		}

		SceneManager.LoadScene("MainMap");
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
		StopAllCoroutines();
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
				if (GetComponentInChildren<SmallRobot>(true) is SmallRobot robot)
				{
					robot.gameObject.SetActive(true);
				}
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