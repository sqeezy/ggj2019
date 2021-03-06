using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace Editor
{
	public class WalkOnGridTest
	{
		private Tile[,] _grid;
		private IEnumerable<Tile> _result;
		private Tile _start;
		private WalkOnGrid _sut;
		private Tile _target;

		[SetUp]
		public void Setup()
		{
			_sut = new GameObject().AddComponent<WalkOnGrid>();
		}

		[TearDown]
		public void Teardown()
		{
			GameObject.DestroyImmediate(_sut.gameObject);
			foreach (var tile in _grid) GameObject.DestroyImmediate(tile.gameObject);
		}

		[Test]
		public void GetPath_finds_the_tile_the_actor_stands_on()
		{
			var tile = new GameObject().AddComponent<Tile>();

			_grid = new[,] {{tile}};
			GivenStart(tile);
			GivenTarget(tile);

			WhenGetPathIsCalled();

			ThenTheSingleResultTileIs(tile);
		}

		[Test]
		[TestCase(1, 0)]
		[TestCase(0, 1)]
		[TestCase(1, 2)]
		[TestCase(2, 1)]
		public void Get_path_finds_tiles_some_paths_away_in_single_direction(int targetX, int targetY)
		{
			GivenGrid(3, 3);
			GivenStart(GridTile(1, 1));
			GivenTarget(GridTile(targetX, targetY));

			WhenGetPathIsCalled();

			ThenResultIs(GridTile(1, 1), GridTile(targetX, targetY));
		}

		/// <summary>
		/// S . .
		/// X X .
		/// . . T
		/// </summary>
		[Test]
		public void GetPath_can_find_its_way_around_obstacles()
		{
			GivenGrid(3, 3);
			GivenStart(GridTile(0, 0));
			GivenTarget(GridTile(2, 2));
			GivenTileIsObstacle(1, 1);
			GivenTileIsObstacle(0, 1);

			WhenGetPathIsCalled();

			ThenResultIs(GridTile(0, 0), GridTile(1, 0), GridTile(2, 0), GridTile(2, 1), GridTile(2, 2));
		}

		/// <summary>
		/// S . . .
		/// X X . X
		/// T . . .
		/// </summary>
		[Test]
		public void GetPath_can_find_through_a_maze()
		{
			GivenGrid(4, 3);
			GivenStart(GridTile(0, 0));
			GivenTarget(GridTile(0, 2));

			GivenTileIsObstacle(1, 1);
			GivenTileIsObstacle(0, 1);
			GivenTileIsObstacle(3, 1);

			WhenGetPathIsCalled();

			ThenResultIs(GridTile(0, 0),
			             GridTile(1, 0),
			             GridTile(2, 0),
			             GridTile(2, 1),
			             GridTile(2, 2),
			             GridTile(1, 2),
			             GridTile(0, 2));
		}

		/// <summary>
		/// S 1 2 3 4
		/// 1 X X X 5
		/// 2 X T X 6
		/// </summary>
		[Test]
		public void GetPath_finds_the_nearest_reachable_tile()
		{
			GivenGrid(5, 3);
			GivenStart(GridTile(0, 0));
			GivenTarget(GridTile(2, 2));

			GivenTileIsObstacle(1, 2);
			GivenTileIsObstacle(1, 1);
			GivenTileIsObstacle(2, 1);
			GivenTileIsObstacle(3, 1);
			GivenTileIsObstacle(3, 2);

			WhenGetPathIsCalled();

			ThenResultIs(GridTile(0, 0), GridTile(0, 1), GridTile(0, 2));
		}

		[Test]
		public void GetPath_can_stay_on_start_when_nothing_is_reachable()
		{
			GivenGrid(2, 1);
			GivenStart(GridTile(1, 0));
			GivenTarget(GridTile(0, 0));

			GivenTileIsObstacle(0, 0);

			WhenGetPathIsCalled();

			ThenResultIs(GridTile(1, 0));
		}

		[Test]
		public void GetPath_doesnt_leave_start_when_its_nearest_reachable_target()
		{
			GivenGrid(2, 2);
			GivenStart(GridTile(0, 1));

			GivenTileIsObstacle(0, 0);
			GivenTarget(GridTile(0, 0));

			WhenGetPathIsCalled();

			ThenResultIs(GridTile(0, 1));
		}

		private void GivenTileIsObstacle(int x, int y)
		{
			_grid[x, y].Walkable = false;
		}

		private void ThenResultIs(params Tile[] expectedPath)
		{
			Assert.True(expectedPath.Length == _result.Count(),
			            $"Got length {_result.Count()}, expected {expectedPath.Length}");

			for (var i = 0; i < expectedPath.Length; i++)
			{
				var equal = expectedPath[i] == _result.ToArray()[i];
				Assert.True(equal);
			}
		}

		private Tile GridTile(int xPos, int yPos)
		{
			return _grid[xPos, yPos];
		}

		private void GivenGrid(int x, int y)
		{
			_grid = new Tile[x, y];
			for (var i = 0; i < x; i++)
			for (var j = 0; j < y; j++)
			{
				_grid[i, j] = new GameObject().AddComponent<Tile>();
				_grid[i, j].X = i;
				_grid[i, j].Y = j;
				_grid[i, j].Walkable = true;
			}
		}

		private void GivenTarget(Tile tile)
		{
			_target = tile;
		}

		private void GivenStart(Tile tile)
		{
			_start = tile;
		}

		private void ThenTheSingleResultTileIs(Tile tile)
		{
			Assert.AreSame(tile, _result.Single());
		}

		private void WhenGetPathIsCalled()
		{
			_sut.Grid = _grid;
			_result = _sut.GetPath(_start, _target);
		}
	}
}