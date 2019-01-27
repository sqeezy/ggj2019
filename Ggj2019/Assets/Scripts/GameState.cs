using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
	public List<GameObject> HomeSweetHome;
	public List<GameObject> HomeSweetHomeDisable;
	public int CurrentUpgradeStep;
	public PlayerActor ActiveActor;
	public PlayerActor CowboyActor;
	public MapInput Input;
	public Map MapState;
	public PlayerActor RobotActor;
	public GameObject Blizzard;

	public List<SpriteRenderer> ShipForDoomsDay;
	
	public int StartEnergy;
	public MainUI UI;
	public int CurrentEnergy { get; set; }

	private void Start()
	{
		Input.GameObjectClicked += InputOnGameObjectClicked;
		Input.GameObjectPickupActionCalled += InputOnGameObjectPickupActionCalled;
		Input.DropActionCalled += InputDropActionCalled;
		RobotActor.EnteredHomeWithUpgrade += OnEnteredHomeWithUpgrade;
		CowboyActor.EnteredHomeWithUpgrade += OnEnteredHomeWithUpgrade;
		RobotActor.EnteredHomeWithShipUpgrades += OnEnteredHomeWithShipUpgrades;
		CowboyActor.EnteredHomeWithShipUpgrades += OnEnteredHomeWithShipUpgrades;
		CurrentEnergy = StartEnergy;
		ActiveActor.EnergyConsumed += ReduceEnergy;
	}

	private void OnEnteredHomeWithShipUpgrades()
	{
		CurrentUpgradeStep++;
		for (int i = 0; i < CurrentUpgradeStep && i < HomeSweetHome.Count; i++)
		{
			if (HomeSweetHomeDisable[i] != null)
			{
				HomeSweetHome[i].SetActive(true);
			}
		}
		
		for (int i = 0; i < CurrentUpgradeStep && i < HomeSweetHomeDisable.Count; i++)
		{
			if (HomeSweetHomeDisable[i]!= null)
			{
				HomeSweetHomeDisable[i].SetActive(false);
			}
		}
		
		
	}

	private void InputDropActionCalled(GameObject obj)
	{
		if (ActiveActor == null)
		{
			return;
		}

		if (ActiveActor.GetComponentInChildren<SmallRobot>() is SmallRobot robi)
		{
			robi.BiteOrSteal(ActiveActor, obj);
		}
		else
		{
			ActiveActor.ActivateTertiaryAbility(ActiveActor, ActiveActor.PositionTile.gameObject);
		}
	}


	private void ReduceEnergy(int amount)
	{
		CurrentEnergy -= amount;
		UI.SetShipBar(CurrentEnergy, StartEnergy);
		if (CurrentEnergy <= 0)
		{
			StartCoroutine(DoomsDay());
		}
	}

	private IEnumerator DoomsDay()
	{
		Input.enabled = false;
		Blizzard.gameObject.SetActive(true);
		var system = Blizzard.GetComponent<ParticleSystem>();
		foreach (var spriteRenderer in ShipForDoomsDay)
		{
			var clr = spriteRenderer.color;
			clr.a = 0;
			
		}
		float time = 6;
		while (time>=0)
		{
			Blizzard.gameObject.SetActive(true);
			system.emissionRate += 100;
			system.startSize += 0.0015f;
			time -= 0.05f;
			foreach (var spriteRenderer in ShipForDoomsDay)
			{
				var clr = spriteRenderer.color; 
				clr.a += 0.05f;
				spriteRenderer.color = clr; 
			}
			
			
			yield return new WaitForSeconds(0.05f);
		}

		SceneManager.LoadScene("MainMap");
	}

	private void InputOnGameObjectPickupActionCalled(GameObject obj)
	{
		if (ActiveActor == null)
		{
			return;
		}

		ActiveActor.ActivateSecondaryAbility(ActiveActor, obj);
	}

	private void InputOnGameObjectClicked(GameObject obj)
	{
		if (obj.GetComponent<Tile>() is Tile tile)
		{
			ClickTileOnActiveActor(tile);
		}
		else if (obj.GetComponent<PlayerActor>() is PlayerActor actor)
		{
			SelectNewActiveActor(actor);
		}
		else if (obj.GetComponent<PushableActor>())
		{
			ActivatePrimaryOnActive(obj);
		}
	}

	private void ActivatePrimaryOnActive(GameObject obj)
	{
		if (ActiveActor == null)
		{
			return;
		}

		ActiveActor.ActivatePrimaryAbility(ActiveActor, obj);
	}

	private void SelectNewActiveActor(PlayerActor actor)
	{
		if (ActiveActor != null)
		{
			ActiveActor.EnergyConsumed -= ReduceEnergy;
			ActiveActor.Deselect();
		}

		ActiveActor = actor;
		ActiveActor.EnergyConsumed += ReduceEnergy;
	}

	private void ClickTileOnActiveActor(Tile tile)
	{
		if (ActiveActor == null)
		{
			return;
		}

		ActiveActor.TargetClicked(tile);
		ActiveActor.TargetConfirmed(tile);
	}

	private void OnEnteredHomeWithUpgrade()
	{
		if ((int) CowboyActor.ActiveUpgrade <= (int) RobotActor.ActiveUpgrade)
		{
			CowboyActor.Upgrade();
		}
		else
		{
			RobotActor.Upgrade();
		}
	}
}