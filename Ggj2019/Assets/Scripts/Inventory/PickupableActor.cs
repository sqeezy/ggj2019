public class PickupableActor : PlayerMovementController
{
	public string CarryAnimationName;
	public string IdleAnimationName;
	
	public virtual void PickUp()
	{
		gameObject.SetActive(false);
		
	}

	public virtual void Drop()
	{
		gameObject.SetActive(true);
	}
}