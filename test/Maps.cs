using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using RogueLike.Definitions;

namespace RogueLike.Tests
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
			var objectLoader = new TestObjectLoader();
			new MapReader(objectLoader);
		}

		[Test]
		public void LoadLevel()
		{
			var objectLoader = new TestObjectLoader();

			var reader = new MapReader(objectLoader);
			var level = reader.LoadLevel("name");

			level.Dimensions.X.Should().Be(4);
			level.Dimensions.Y.Should().Be(4);
		}

		[Test]
		public void SaveMap()
		{
			var small = Data.Maps.Small().Save();
			small.Tiles.Should().BeEmpty();

			var door = Data.Maps.HasDoor().Save();
			door.Tiles.Should().BeEmpty();

			var map = Data.Maps.HasDoor();
			map.OpenDoor(Data.Maps.DoorLocation).Should().BeTrue();
			map[Data.Maps.DoorLocation].Should().Be(Tiles.OpenDoor);

			var openDoorTiles = map.Save();
			openDoorTiles.Tiles.Should().NotBeEmpty();
			var openDoor = openDoorTiles.Tiles.Single();
			openDoor.Coordinates.X.Should().Be(Data.Maps.DoorLocation.X);
			openDoor.Coordinates.Y.Should().Be(Data.Maps.DoorLocation.Y);
		}

		[Test]
		public void LoadMap()
		{
			var map = Data.Maps.HasDoor();

			map.Load(new MapState());
			map[Data.Maps.DoorLocation].Should().Be(Tiles.ClosedDoor);

			map.Load(new MapState { Tiles = { new DoorTileState { Coordinates = new PointXY { X = Data.Maps.DoorLocation.X, Y = Data.Maps.DoorLocation.Y }, IsOpen = true } } });
			map[Data.Maps.DoorLocation].Should().Be(Tiles.OpenDoor);

			map = Data.Maps.HasMob();

			map.Load(new MapState());
			map.Mobiles.SingleOrDefault(m => m.Position == Data.Maps.MobLocation).Should().NotBeNull();

			PointXY updatedLocation = new PointXY() { X = 1, Y = 1 };
			map.Load(new MapState { Mobiles = { new MobileState() { Id = 0, Position = updatedLocation } }});
			map.Mobiles.SingleOrDefault(m => m.Position == Data.Maps.MobLocation).Should().BeNull();
			map.Mobiles.SingleOrDefault(m => m.Position == updatedLocation).Should().NotBeNull();
		}

		private class TestObjectLoader : IObjectLoader
		{
			public T Load<T>(string name)
			{
				return (T) (object) Data.Level;
			}

			public List<T> LoadAll<T>(string name)
			{
				return (List<T>) (object) Data.Monsters;
			}

			public char[][] LoadTiles(string mapName)
			{
				return Data.Tiles.Small;
			}
		}
	}

	static class EnumerableExtensions
	{
		public static IEnumerable<T> AsEnumerable<T>(this T source)
		{
			yield return source;
		}
	}
}