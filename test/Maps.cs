using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Xunit;

namespace RogueLike.Tests
{
	public class Maps
	{
		[Theory]
		[InlineData(-1, -1, true)]
		[InlineData(-1, 1, true)]
		[InlineData(1, -1, true)]
		[InlineData(0, 0, true)]
		[InlineData(10, 10, true)]
		[InlineData(1, 1, false)]
		public void OutOfBounds(int x, int y, bool outOfBounds)
		{
			var map = Data.Maps.Small();
			map.IsOccupied(new Point(x, y)).ShouldBe(outOfBounds);
		}
	}

	public class MapReading
	{
		[Fact]
		public void CanCreateMapReader()
		{
			var objectLoader = new TestObjectLoader();
			_ = new MapReader(objectLoader);
		}

		[Fact]
		public void LoadLevel()
		{
			var objectLoader = new TestObjectLoader();

			var reader = new MapReader(objectLoader);
			var level = reader.LoadLevel("name");

			var (x, y) = level.Dimensions;
			(x, y).ShouldBe((4, 4));
		}

		[Fact]
		public void SaveMap()
		{
			var small = Data.Maps.Small().Save();
			small.Tiles.ShouldBeEmpty();

			var door = Data.Maps.HasDoor().Save();
			door.Tiles.ShouldBeEmpty();

			var map = Data.Maps.HasDoor();
			map.OpenDoor(Data.Maps.DoorLocation).ShouldBeTrue();
			map[Data.Maps.DoorLocation].ShouldBe(Tiles.OpenDoor);

			var openDoorTiles = map.Save();
			openDoorTiles.Tiles.ShouldNotBeEmpty();
			var openDoor = openDoorTiles.Tiles.Single();
			openDoor.Coordinates.X.ShouldBe(Data.Maps.DoorLocation.X);
			openDoor.Coordinates.Y.ShouldBe(Data.Maps.DoorLocation.Y);
		}

		[Fact]
		public void LoadMap()
		{
			var map = Data.Maps.HasDoor();

			map.Load(new MapState());
			map[Data.Maps.DoorLocation].ShouldBe(Tiles.ClosedDoor);

			var door = new PointXY(Data.Maps.DoorLocation.X, Data.Maps.DoorLocation.Y);
			map.Load(new MapState { Tiles = { new DoorTileState { Coordinates = door, IsOpen = true } } });
			map[Data.Maps.DoorLocation].ShouldBe(Tiles.OpenDoor);

			map = Data.Maps.HasMob();

			map.Load(new MapState());
			map.Mobiles.SingleOrDefault(m => m.Position == Data.Maps.MobLocation).ShouldNotBeNull();

			var updatedLocation = new PointXY(1, 1);
			map.Load(new MapState { Mobiles = { new MobileState() { Id = 0, Position = updatedLocation } }});
			map.Mobiles.SingleOrDefault(m => m.Position == Data.Maps.MobLocation).ShouldBeNull();
			map.Mobiles.SingleOrDefault(m => m.Position == updatedLocation).ShouldNotBeNull();
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
}