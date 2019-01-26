using System;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
	private UpgradeState CurrentUpgrade;
	
	public Animator AnimationController;
	public RuntimeAnimatorController Upgrade1Controller;
	public RuntimeAnimatorController Upgrade2Controller;

	public AnimationNames AnimationData;

	public void Move()
	{
		AnimationController.SetTrigger(AnimationData.Move);
	}

	public void Idle()
	{
		AnimationController.SetTrigger(AnimationData.Idle);
	}

	public void BasicAbility()
	{
		AnimationController.SetTrigger(AnimationData.BasicAbility);
	}

	public void Reset()
	{
		AnimationController.ResetTrigger(AnimationData.Move);
		AnimationController.ResetTrigger(AnimationData.Idle);
		AnimationController.ResetTrigger(AnimationData.BasicAbility);
		AnimationController.ResetTrigger(AnimationData.SecondaryAbility);
	}

	public void Upgrade()
	{
		switch (CurrentUpgrade)
		{
			case UpgradeState.NoUpgrade:
				AnimationController.runtimeAnimatorController = Upgrade1Controller;
				CurrentUpgrade = UpgradeState.Upgrade1;
				break;
			case UpgradeState.Upgrade1:
				AnimationController.runtimeAnimatorController = Upgrade2Controller;
				CurrentUpgrade = UpgradeState.Upgrade2;
				break; 
		}

	}

	[Serializable]
	public struct AnimationNames
	{
		public string BasicAbility;
		public string SecondaryAbility;
		public string Move;
		public string Idle;
	}
}