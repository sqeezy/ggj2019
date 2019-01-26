using System;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
	public event Action<int> EnergyConsumed = (t) => { };

	public Ability BasicAbility;
	
	public WalkOnGrid WalkOnGrid;

	public IEnumerable<Tile> Path { get; protected set; }

	public virtual void Deselect()
	{
	}

	public virtual void TargetClicked(Tile target)
	{
	}

	public virtual void TargetConfirmed(Tile tile)
	{
	}

	protected void ConsumeEnergy(int amount)
	{
		EnergyConsumed(amount);
	}

	protected void ActivateBasicAbility(Tile targetTile)
	{
		if (BasicAbility != null)
		{
			BasicAbility.Do(targetTile);
			ConsumeEnergy(BasicAbility.EnergyAmount);
		}
	}
}