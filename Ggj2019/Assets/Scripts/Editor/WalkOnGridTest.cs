using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Editor
{
    public class WalkOnGridTest
    {
        private WalkOnGrid _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new WalkOnGrid();
        }

        [Test]
        public void GetPath_finds_the_tile_the_actor_stands_on()
        {
            var allTheSame = new Tile();

            var grid = new Tile[1, 1] {{allTheSame}};
            var start = allTheSame;
            var target = allTheSame;

            IEnumerable<Tile> result = _sut.GetPath(grid, start, target);

            Assert.AreSame(allTheSame, result.Single());
        }
    }
}