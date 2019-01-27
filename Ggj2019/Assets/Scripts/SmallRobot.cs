using UnityEngine;

public class SmallRobot : PlayerActor
{
	private Vector3 _parentPosition;
	private bool _push;
	private bool _returning;
	private Vector3 _targetPosition;
	public PickUpAbility PickUpAbility;
	public PushAbility Push;
	private GameObject _target;

	protected override void Start()
	{
	}

	public void FlyAndBite(PlayerActor activeActor, GameObject target)
	{
		_parentPosition = activeActor.transform.position;
		var direction = target.transform.position - _parentPosition;

		_push = true;
		_returning = false;
		_targetPosition = transform.position + direction.normalized * 2;
		_target = target;
	}

	private void Update()
	{
		if (_push || _returning)
		{
			_targetPosition.z = transform.position.z;
			var currentDirection = _targetPosition - transform.position;
			var currentDistance = currentDirection.magnitude;

			if (currentDistance <= 0.1f)
			{
				if (_push)
				{
					Push.Do(_target);
					_push = false;
				}

				_returning = !_returning;
				_targetPosition = _parentPosition;
				return;
			}

			var tempDirection = currentDirection.normalized;
			tempDirection *= Speed * Time.deltaTime;
			transform.position += tempDirection;
		}
	}
}