using UnityEngine;

public class GameState : MonoBehaviour
{
	public PlayerActor ActiveActor;
	public PlayerActor CowboyActor;
	public MapInput Input;
	public Map MapState;
	public PlayerActor RobotActor;

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
		CurrentEnergy = StartEnergy;
		ActiveActor.EnergyConsumed += ReduceEnergy;
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