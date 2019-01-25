
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
	private IEnumerable<Tile> TilePath;

    // Update is called once per frame
    void Update()
    {
        
    }

	public void MoveOnPath(IEnumerable<Tile> path)
	{
		TilePath = path;
	}
}
