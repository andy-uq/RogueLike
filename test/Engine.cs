using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LanguageExt;
using NUnit.Framework;

using static LanguageExt.Prelude;

namespace RogueLike.Tests
{
	class Engine
	{
		[Test]
		public void StartEngine()
		{
			MakeEngine();
		}

		[Test]
		public void SetStatusLine()
		{
			var engine = MakeEngine();
			engine.SetStatus("Hello world");
			engine.StatusLine.Should().Be("Hello world");
			engine.StatusTtl.Should().BeGreaterThan(0);
		}

		[Test]
		public void StatusLineClears()
		{
			var engine = MakeEngine();
			engine.SetStatus("Hello world");

			var ttl = engine.StatusTtl;
			while (ttl > 0)
			{
				var status = engine.GetStatusLine();
				status.IsNone.Should().BeFalse();
				status.IfSome(s => s.Should().Be("Hello world"));
				ttl--;
			}

			engine.GetStatusLine().IsNone.Should().BeTrue();
			engine.StatusTtl.Should().Be(0);
		}

		[Test]
		public async Task QueueNextAction()
		{
			var engine = MakeEngine();
			var movePlayerAction = new MovePlayerAction(new Point(1, 1));
			await engine.EnqueueActionAsync(movePlayerAction);
			var task = await engine.TakeNextActionAsync();
			task.Should().Be(movePlayerAction);
		}

		[Test]
		public async Task EndGameEndsQueue()
		{
			var engine = MakeEngine();
			var ended = false;

			engine.IsActive.Should().BeTrue();

			var t = Task.Run(async () =>
			{
				while (engine.IsActive)
				{
					await engine.TakeNextActionAsync();
				}

				ended = true;
			});


			engine.EndGame();
			await t;

			ended.Should().BeTrue();
		}

		[Test]
		public void SaveGame()
		{
			var store = new TestSaveGameStore();

			var engine = MakeEngine();
			engine.SaveGameStore = store;
			engine.Map = Data.Maps.Small();
			engine.Player = new Player() { Position = new Point(1, 2) };
			engine.Save();

			store.Player.Position.X.Should().Be(engine.Player.Position.X);
			store.Player.Position.Y.Should().Be(engine.Player.Position.Y);
			store.Map.Tiles.Should().BeEmpty();
		}

		[Test]
		public void LoadGame()
		{
			var store = new TestSaveGameStore();

			var engine = MakeEngine();
			engine.Map = Data.Maps.Small();
			engine.SaveGameStore = store;

			engine.Load();

			engine.Player.Position.X.Should().Be(store.Player.Position.X);
			engine.Player.Position.Y.Should().Be(store.Player.Position.Y);
			engine.Map.Should().NotBeNull();
		}

		private static GameEngine MakeEngine()
		{
			return new GameEngine { IsActive = true };
		}
	}

	internal class TestSaveGameStore : ISaveGameStore
	{
		private Option<PlayerState> _player;
		private Option<MapState> _map;

		public TestSaveGameStore(Option<PlayerState> player = default(Option<PlayerState>))
		{
			_player = player;
			_map = None;
		}

		public PlayerState Player => _player.IfNone(new PlayerState());
		public MapState Map => _map.IfNone(new MapState());

		public Option<PlayerState> LoadPlayer()
		{
			return Player;
		}

		public Option<MapState> LoadMap()
		{
			return Map;
		}

		public void Save(PlayerState player)
		{
			_player = player;
		}

		public void Save(MapState map)
		{
			_map = map;
		}
	}
}
