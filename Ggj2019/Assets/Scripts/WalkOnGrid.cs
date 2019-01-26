#region

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#endregion

public class WalkOnGrid : MonoBehaviour
{
	public Tile[,] Grid;
	public Map Map;

	private void Start()
	{
		Map.Load();
		Grid = Map.Grid;
	}

	public IEnumerable<Tile> GetPath(Tile start, Tile target)
	{
		if (start == target)
		{
			return new[] {start};
		}

		var allTiles = Grid.Flatten().ToArray();
		var distances = allTiles.ToDictionary(t => t, l => 100_000);
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

			foreach (var neighbor in nodeWithShortestDistance.GetNeighbors(Grid).Intersect(stillToVisit))
				if (distances[neighbor] > distCandidate)
				{
					distances[neighbor] = distCandidate;
					bestPrev[neighbor] = nodeWithShortestDistance;
				}
		}

		var result = new List<Tile>();

		var targetReached = bestPrev[target] != null;
		if (targetReached)
		{
			result = BacktrackFromTarget(target, bestPrev);
		}
		else
		{
			var reachableTiles = bestPrev.Where(l => l.Value != null);
			if (!reachableTiles.Any())
			{
				result.Add(start);
			}
			else
			{
				var hitTilesNearestToTarget = reachableTiles.Select(l => l.Key)
				                                            .Concat(new[] {start})
				                                            .GroupBy(h => Distance(h, target))
				                                            .OrderBy(g => g.Key)
				                                            .First();
				var nearTargetHitWithBestWayToStart =
					hitTilesNearestToTarget.OrderBy(l => distances[l]).FirstOrDefault();
				result = BacktrackFromTarget(nearTargetHitWithBestWayToStart, bestPrev);
			}
		}

		result.Reverse();

		return result;
	}

	private static float Distance(Tile t1, Tile t2)
	{
		return (new Vector2Int(t1.X, t1.Y) - new Vector2Int(t2.X, t2.Y)).magnitude;
	}

	private static List<Tile> BacktrackFromTarget(Tile target, Dictionary<Tile, Tile> bestPrev)
	{
		var r = new List<Tile>();
		var currentTile = target;
		while (currentTile != null)
		{
			r.Add(currentTile);
			currentTile = bestPrev[currentTile];
		}

		return r;
	}
}