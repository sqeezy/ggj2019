#region

using System.Collections.Generic;
using UnityEngine;

#endregion

public class Actor : MonoBehaviour
{
	public WalkOnGrid WalkOnGrid;

	public IEnumerable<Tile> Path { get; protected set; }

	public virtual void TargetClicked(Tile target)
	{
	}

	public virtual void TargetConfirmed(Tile tile)
	{
	}
}