using System.Collections.Generic;
using carl;
using carl.Definitions;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace test
{
	[TestFixture]
	public class Maps
	{
		[TestCase(-1, -1, true)]
		[TestCase(-1, 1, true)]
		[TestCase(1, -1, true)]
		[TestCase(0, 0, true)]
		[TestCase(10, 10, true)]
		[TestCase(1, 1, false)]
		public void OutOfBounds(int x, int y, bool outOfBounds)
		{
			var map = Data.Maps.Small();
			map.IsOccupied(new Point(x, y)).Should().Be(outOfBounds);
		}
	}

	public class MapReading
	{
		[Test]
		public void CanCreateMapReader()
		{
			CreateReader(Mock.Of<IGameEngine>());
		}

		private static MapReader CreateReader(IGameEngine gameEngine)
		{
			return new MapReader(gameEngine);
		}

		[Test]
		public void LoadLevel()
		{
			var objectLoader = new Mock<IObjectLoader>(MockBehavior.Strict);
			objectLoader.Setup(x => x.Load<Level>("name")).Returns(new Level() {Map = "map", Monsters = "monsters"});
			objectLoader.Setup(x => x.LoadAll<Monster>("monsters")).Returns(new Monster() {}.ToList());
			objectLoader.Setup(x => x.LoadTiles("map")).Returns(Data.Tiles.Small);

			var gameEngine = new Mock<IGameEngine>(MockBehavior.Strict);
			gameEngine.SetupGet(x => x.ObjectLoader).Returns(objectLoader.Object);

			var reader = CreateReader(gameEngine.Object);
			var level = reader.LoadLevel("name");

			level.Dimensions.X.Should().Be(4);
			level.Dimensions.Y.Should().Be(4);
		}

	}

	static class EnumerableExtensions
	{
		public static IEnumerable<T> AsEnumerable<T>(this T source)
		{
			yield return source;
		}
		public static List<T> ToList<T>(this T source)
		{
			return new List<T>() { source };
		}
	}
}