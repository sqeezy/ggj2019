﻿using System;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
	public Animator AnimationController;

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

	[Serializable]
	public struct AnimationNames
	{
		public string BasicAbility;
		public string SecondaryAbility;
		public string Move;
		public string Idle;
	}
}