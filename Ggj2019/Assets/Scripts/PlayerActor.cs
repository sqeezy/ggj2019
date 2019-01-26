using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class PlayerActor : PlayerMovementController
{
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
}
