using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class PlayerActor : PlayerMovementController
{
	public int FullEnergy;
	public int MaxEnergy;
	public int CurrentEnergy;
	public CharacterAnimation AnimationController;

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
