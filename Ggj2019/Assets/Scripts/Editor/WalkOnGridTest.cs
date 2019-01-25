using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace Editor
{
	public class WalkOnGridTest
	{
		private WalkOnGrid _sut;
		private IEnumerable<Tile> _result;
		private Tile _start;
		private Tile _target;
		private Tile[,] _grid;

		[SetUp]
		public void Setup()
		{
			_sut = new WalkOnGrid();
		}

		[TearDown]
		public void Teardown()
		{
			foreach (var tile in _grid)
			{
				GameObject.DestroyImmediate(tile.gameObject);
			}
		}

		[Test]
		public void GetPath_finds_the_tile_the_actor_stands_on()
		{
			var tile = new GameObject().AddComponent<Tile>();

			_grid = new Tile[1, 1] {{tile}};
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

			var expectedResult = new[] {GridTile(1, 1), GridTile(targetX, targetY)};

			CollectionAssert.AreEqual(expectedResult, _result);
		}

		private Tile GridTile(int xPos, int yPos)
		{
			return _grid[xPos, yPos];
		}

		private void GivenGrid(int x, int y)
		{
			_grid = new Tile[x, y];
			for (int i = 0; i < x; i++)
			{
				for (int j = 0; j < y; j++)
				{
					_grid[i, j] = new GameObject().AddComponent<Tile>();
					_grid[i, j].X = i;
					_grid[i, j].Y = j;
				}
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
			_result = _sut.GetPath(_grid, _start, _target);
		}
	}
}