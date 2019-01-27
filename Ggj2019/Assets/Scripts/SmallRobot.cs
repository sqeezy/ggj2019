using UnityEngine;

public class SmallRobot : PlayerActor
{
	public Ability _activeAbility;
	private bool _attacking;
	private PlayerActor _parent;
	private Vector3 _parentPosition;
	private bool _returning;
	private GameObject _target;
	private Vector3 _targetPosition;
	public PickUpAbility Pick;
	public PushAbility Push;

	protected override void Start()
	{
	}

	public void BiteOrSteal(PlayerActor activeActor, GameObject target)
	{
		if (target.GetComponent<PushableActor>())
		{
			_activeAbility = Push;
			Armed(activeActor, target);
			AnimationController.Move();
		}
		else
		{
			if (target == activeActor)
			{
				return;
			}

			_activeAbility = Pick;
			Armed(activeActor, target);
		}
	}

	private void Armed(PlayerActor activeActor, GameObject target)
	{
		_parent = activeActor;
		_parentPosition = _parent.transform.position;
		var direction = target.transform.position - _parentPosition;

		_attacking = true;
		_returning = false;
		_targetPosition = transform.position + direction.normalized * 2;
		_target = target;
		AnimationController.Move();
	}

	private void Update()
	{
		if (_attacking || _returning)
		{
			_targetPosition.z = transform.position.z;
			var currentDirection = _targetPosition - transform.position;
			var currentDistance = currentDirection.magnitude;

			if (currentDistance <= 0.3f)
			{
				if (_attacking)
				{
					_activeAbility.Do(_target);
					AnimationController.Move();
					_attacking = false;
				}

				_returning = !_returning;
				_targetPosition = _parentPosition;
				if (!(_attacking || _returning))
				{
					AnimationController.Idle();
					if (_activeAbility == Pick)
					{
						_parent.CarriedPickupableActor = CarriedPickupableActor;
						CarriedPickupableActor = null;
					}
				}

				return;
			}

			var tempDirection = currentDirection.normalized;
			tempDirection *= Speed * Time.deltaTime;
			transform.position += tempDirection;
		}
	}
}