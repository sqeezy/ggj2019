﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WalkOnGrid : MonoBehaviour
{
	private Tile[,] _grid;

	private void Start()
	{
		_grid = GetComponent<Map>().Grid;
	}

	public IEnumerable<Tile> GetPath(Tile start, Tile target)
	{
		if (start == target)
		{
			return new[] {start};
		}

		var allTiles = _grid.Flatten().ToArray();
		var distances = allTiles.ToDictionary(t => t, l => int.MaxValue);
		var bestPrev = allTiles.ToDictionary(t => t, l => (Tile) null);
		var stillToVisit = new HashSet<Tile>(allTiles);
		distances[start] = 0;

		while (stillToVisit.Any())
		{
			var nodeWithShortestDistance =
				distances.Where(l => stillToVisit.Contains(l.Key)).OrderBy(l => l.Value).First().Key;
			var nodeDistance = distances[nodeWithShortestDistance];
			stillToVisit.Remove(nodeWithShortestDistance);
			var distCandidate = nodeDistance + 1;

			foreach (var neighbor in nodeWithShortestDistance.GetNeighbors(_grid).Intersect(stillToVisit))
			{
				if (distances[neighbor] > distCandidate)
				{
					distances[neighbor] = distCandidate;
					bestPrev[neighbor] = nodeWithShortestDistance;
				}
			}
		}

		var currentTile = target;
		var result = new List<Tile>();
		if (bestPrev[currentTile] != null || currentTile == start)
		{
			while (currentTile != null)
			{
				result.Add(currentTile);
				currentTile = bestPrev[currentTile];
			}
		}

		result.Reverse();

		return result;
	}
}