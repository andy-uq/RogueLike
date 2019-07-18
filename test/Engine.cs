using System;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace RogueLike.Tests
{
	public class Engine
	{
		[Fact]
		public void StartEngine()
		{
			MakeEngine();
		}

		[Fact]
		public void SetStatusLine()
		{
			var engine = MakeEngine();
			engine.SetStatus("Hello world");
			engine.StatusLine.ShouldBe("Hello world");
			engine.StatusTtl.ShouldBeGreaterThan(0);
		}

		[Fact]
		public void StatusLineClears()
		{
			var engine = MakeEngine();
			engine.SetStatus("Hello world");

			var ttl = engine.StatusTtl;
			while (ttl > 0)
			{
				var status = engine.GetStatusLine();
				status.ShouldBe("Hello world");
				ttl--;
			}

			engine.GetStatusLine().ShouldBeNull();
			engine.StatusTtl.ShouldBe(0);
		}

		[Fact]
		public async Task QueueNextAction()
		{
			var engine = MakeEngine();
			var context = new GameActionContext(engine);

			var movePlayerAction = new MovePlayerAction(new Point(1, 1));
			
			await engine.EnqueueActionAsync(movePlayerAction);
			await engine.TakeNextActionAsync(context);

		}

		[Fact]
		public async Task EndGameEndsQueue()
		{
			var engine = MakeEngine();
			var context = new GameActionContext(engine);

			var ended = false;

			engine.IsActive.ShouldBeTrue();

			var t = Task.Run(async () =>
			{
				while (engine.IsActive)
				{
					await engine.TakeNextActionAsync(context);
				}

				ended = true;
			});


			engine.EndGame();
			await t;

			ended.ShouldBeTrue();
		}

		[Fact]
		public void SaveGame()
		{
			var store = new TestSaveGameStore();

			var engine = MakeEngine(store);
			engine.Map = Data.Maps.Small();
			engine.Player = new Player() { Position = new Point(1, 2) };
			engine.Save();

			store.Player.Position.X.ShouldBe(engine.Player.Position.X);
			store.Player.Position.Y.ShouldBe(engine.Player.Position.Y);
			store.Map.Tiles.ShouldBeEmpty();
		}

		[Fact]
		public void LoadGame()
		{
			var store = new TestSaveGameStore();

			var engine = MakeEngine(store);
			engine.Map = Data.Maps.Small();

			engine.Load();

			engine.Player.Position.X.ShouldBe(store.Player.Position.X);
			engine.Player.Position.Y.ShouldBe(store.Player.Position.Y);
			engine.Map.ShouldNotBeNull();
		}

		private static GameEngine MakeEngine(ISaveGameStore? store = null)
		{
			return new GameEngine(store ?? new TestSaveGameStore());
		}
	}

	internal class TestSaveGameStore : ISaveGameStore
	{
		private PlayerState? _player;
		private MapState? _map;

		public TestSaveGameStore(PlayerState? player = default)
		{
			_player = player;
			_map = null;
		}

		public PlayerState Player => _player ?? new PlayerState();
		public MapState Map => _map ?? new MapState();

		public PlayerState? LoadPlayer(Action<PlayerState> action)
		{
			if (Player != null)
			{
				action(Player);
			}

			return Player;
		}

		public MapState? LoadMap(Action<MapState> action)
		{
			if (Map != null)
			{
				action(Map);
			}

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
