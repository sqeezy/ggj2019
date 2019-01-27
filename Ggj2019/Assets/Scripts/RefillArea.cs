using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class RefillArea : MonoBehaviour
{
		private void OnTriggerStay(Collider other)
		{
			if (other.GetComponent<PlayerActor>() is PlayerActor actor)
			{
				actor.RefillToFull();
			}
		}
}