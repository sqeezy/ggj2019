using System.Collections;
using UnityEngine;

public class PushAbility : Ability
{
	public CharacterAnimation AnimationController;
	public float AnimationOffset;

	public override void Do(GameObject targetTile)
	{
		var actor = targetTile.GetComponent<PushableActor>();
		if (actor != null)
		{
			var direction = (actor.transform.position - transform.position).normalized;
			StopAllCoroutines();
			StartCoroutine(WaitForPush(actor, direction));
			var scale = transform.localScale;
			if (direction.x < 0)
			{
				scale.x = -1;
			}
			else
			{
				scale.x = 1;
			}

			transform.localScale = scale;
			if (AnimationController != null)
			{
				AnimationController.BasicAbility();
			}
		}
		else
		{
			if (AnimationController != null)
			{
				AnimationController.BasicAbility();
			}
		}
	}

	private IEnumerator WaitForPush(PushableActor actor, Vector3 direction)
	{
		yield return new WaitForSeconds(AnimationOffset);
		actor.Push(direction);
	}
}