﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushAbility : Ability
{
	public CharacterAnimation AnimationController;
	
	public override void Do(GameObject targetTile)
	{
		var actor = targetTile.GetComponent<PushableActor>();
		if (actor != null)
		{
			var direction = actor.transform.position - this.transform.position;
			actor.Push(direction);
			if (AnimationController != null)
			{
				AnimationController.BasicAbility();
			}
		}
	}
}