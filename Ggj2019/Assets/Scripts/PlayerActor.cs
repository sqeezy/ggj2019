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
	public GameObject CowboyUpgradeParticleEffect;
	public GameObject RobotUpgradeParticleEffect;

	public SpriteRenderer RobotSprite;
	public int CowboySortingLayer;
	private int _originalRobotSortingLayer;

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
	public event Action EnteredHomeWithShipUpgrades;

	private void Start()
	{
		base.Start();
		if (IsRobot)
		{
			_originalRobotSortingLayer = RobotSprite.sortingOrder;
		} 
	}

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
				if (IsRobot)
				{
					if(GetComponent<BoxCollider>() is BoxCollider collider)
					{
						collider.size = new Vector3(1f, 1f, .5f);
					}
					RobotSprite.sortingOrder = CowboySortingLayer - 1;
				}
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

			if (CarriedPickupableActor != null && CarriedPickupableActor.UpgradeShipActors)
			{
				var tmp = CarriedPickupableActor;
				Destroy(tmp.gameObject);
				AnimationController.Reset();
				AnimationController.Idle();
				EnteredHomeWithShipUpgrades.Raise();
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
		if (IsRobot)
		{
			// reset sorting layer and z-pos.
			if (GetComponent<BoxCollider>() is BoxCollider collider)
			{
				collider.size = new Vector3(1f, 1f, 1f);
			}
			RobotSprite.sortingOrder = _originalRobotSortingLayer;
		}
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
				else if (IsRobot)
				{
					if (RobotUpgradeParticleEffect != null)
					{
						RobotUpgradeParticleEffect.SetActive(true);
					}
				}
				else
				{
					if (CowboyUpgradeParticleEffect != null)
					{
						CowboyUpgradeParticleEffect.SetActive(true);
					}
				}
				break;
			case UpgradeState.Upgrade1:
				AnimationController.Upgrade();
				ActiveUpgrade = UpgradeState.Upgrade2;
				if (IsRobot)
				{
					if (RobotUpgradeParticleEffect != null)
					{
						RobotUpgradeParticleEffect.SetActive(true);
					}
				}
				else
				{
					if (CowboyUpgradeParticleEffect != null)
					{
						CowboyUpgradeParticleEffect.SetActive(true);
					}
				}
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