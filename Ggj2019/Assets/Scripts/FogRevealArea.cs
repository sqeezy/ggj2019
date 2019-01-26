using UnityEngine;

namespace DefaultNamespace
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(SphereCollider))]
	public class FogRevealArea : MonoBehaviour
	{
		private Rigidbody _rigidBody;

		private void Start()
		{
			_rigidBody = GetComponent<Rigidbody>();
			_rigidBody.useGravity = false;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.GetComponent<Revealable>() is Revealable rev)
			{
				rev.Reveal();
			}
		}
	}
}