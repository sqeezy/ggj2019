using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
	public Animator AnimationController;

	[Serializable]
	public struct AnimationNames
	{
		public string BasicAbility;
		public string SecondaryAbility;
		public string Move;
		public string Idle;
	}

	public AnimationNames AnimationData;
	public void Move()
	{
		AnimationController.SetTrigger(AnimationData.Move);
	}
}