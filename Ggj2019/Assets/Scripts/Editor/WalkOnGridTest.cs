using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

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

        [Test]
        public void GetPath_finds_the_tile_the_actor_stands_on()
        {
            var tile = new Tile();

            _grid = new Tile[1, 1] {{tile}};
            GivenStart(tile);
            GivenTarget(tile);

            WhenGetPathIsCalled();

            ThenTheSingleResultTileIs(tile);
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