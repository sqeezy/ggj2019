using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableRobot : PickupableActor
{
	public CharacterAnimation AnimationController;
	public override void PickUp()
	{
		//base.PickUp(); // avoid hide instant
		
		StopAllCoroutines();
		AnimationController.SecondaryAbility();
		StartCoroutine(WaitForAnimation());
	}

	private IEnumerator WaitForAnimation()
	{
		yield return new WaitForSeconds(1);
		gameObject.SetActive(false);
	}

	public override void Drop()
	{
		base.Drop();
		AnimationController.ThirdAbility();
	}
}